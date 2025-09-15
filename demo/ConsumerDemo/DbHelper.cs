using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace ConsumerDemo
{
    public class DbHelper
    {
        const string sql = @"
INSERT INTO public.calender(account, debit_credit, amount)
VALUES ($1, $2, $3);";

        const string pgHost = "localhost";
        const string pgPort = "5432";
        const string pgUser = "debezium";
        const string pgPass = "debezium";
        const string pgDb = "corebank";
        const string pgConn = $"Host={pgHost};Port={pgPort};Username={pgUser};Password={pgPass};Database={pgDb};Pooling=true;Maximum Pool Size=50;";


        public async static Task<bool> Save(MoneyTransfer moneyTransfer, CancellationToken cancellationToken)
        {
            await using var conn = new NpgsqlConnection(pgConn);
            await conn.OpenAsync(cancellationToken);

            await using var tx = await conn.BeginTransactionAsync(cancellationToken);
            try
            {
                await using (var cmd = new NpgsqlCommand(sql, conn, tx))
                {
                    // From -> debit_credit = true
                    cmd.Parameters.AddWithValue(moneyTransfer.From);
                    cmd.Parameters.AddWithValue(true);
                    cmd.Parameters.AddWithValue(moneyTransfer.Amount);
                    await cmd.ExecuteNonQueryAsync(cancellationToken);
                    cmd.Parameters.Clear();

                    // To -> debit_credit = false
                    cmd.Parameters.AddWithValue(moneyTransfer.To);
                    cmd.Parameters.AddWithValue(false);
                    cmd.Parameters.AddWithValue(moneyTransfer.Amount);
                    await cmd.ExecuteNonQueryAsync(cancellationToken);
                }

                await tx.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(cancellationToken);
                Console.WriteLine($"db-write-failed: {ex.Message}");
                
                return false;
            }
        }
    }
}
