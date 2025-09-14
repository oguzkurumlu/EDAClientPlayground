Things to do

- A Chain of Resp for delivering a message to Kafka
- Main purpose is to deliver the message to Kafka
- If Kafka deliver fails, try rabbit mq
- If rabbit mq fails, try file logger
- Collect data on rabbitmq and log via kafka connect



# Kafka connect definition

# rabbit primitive
curl -X PUT http://localhost:8083/connectors/rmq-source/config \
  -H "Content-Type: application/json" \
  -d '{
    "connector.class": "com.github.jcustenborder.kafka.connect.rabbitmq.RabbitMQSourceConnector",
    "tasks.max": "1",
    "rabbitmq.host": "rabbitmq",
    "rabbitmq.port": "5672",
    "rabbitmq.username": "guest",
    "rabbitmq.password": "guest",
    "rabbitmq.virtual.host": "/",
    "rabbitmq.queue": "events",
    "kafka.topic": "rmq.events",
    "value.converter": "org.apache.kafka.connect.storage.StringConverter",
    "key.converter": "org.apache.kafka.connect.storage.StringConverter"
  }'


# rabbit
  curl -X PUT http://localhost:8083/connectors/rmq-source/config \
  -H "Content-Type: application/json" \
  -d '{
    "connector.class": "com.github.jcustenborder.kafka.connect.rabbitmq.RabbitMQSourceConnector",
    "tasks.max": "1",
    "rabbitmq.host": "rabbitmq",
    "rabbitmq.port": "5672",
    "rabbitmq.username": "guest",
    "rabbitmq.password": "guest",
    "rabbitmq.virtual.host": "/",
    "rabbitmq.queue": "kafka_events",
    "kafka.topic": "rmq.events",

    "key.converter": "org.apache.kafka.connect.storage.StringConverter",
    "value.converter": "org.apache.kafka.connect.storage.StringConverter",

    "transforms": "k1",
    "transforms.k1.type": "com.github.jcustenborder.kafka.connect.rabbitmq.ExtractHeader$Key",
    "transforms.k1.header.name": "kafka_key"
  }'

 # file

curl -X PUT "http://localhost:8083/connectors/file-source/config" \
  -H "Content-Type: application/json" \
  --data-binary '{
    "connector.class": "org.apache.kafka.connect.file.FileStreamSourceConnector",
    "tasks.max": "1",
    "file": "/data/input.txt",
    "topic": "rmq.events",

    "key.converter": "org.apache.kafka.connect.storage.StringConverter",
    "value.converter": "org.apache.kafka.connect.json.JsonConverter",
    "value.converter.schemas.enable": "false",

    "transforms": "fromJson,makeKey,extractKey",
    "transforms.fromJson.type": "com.github.jcustenborder.kafka.connect.json.FromJson$Value",
    "transforms.fromJson.json.schema.location": "Inline",
    "transforms.fromJson.json.schema.inline":
      "{ \"type\":\"object\", \"properties\": { \"Key\": {\"type\":\"string\"}, \"Message\": {\"type\":\"string\"} }, \"required\": [\"Key\",\"Message\"] }",

    "transforms.makeKey.type": "org.apache.kafka.connect.transforms.ValueToKey",
    "transforms.makeKey.fields": "Key",
    "transforms.extractKey.type": "org.apache.kafka.connect.transforms.ExtractField$Key",
    "transforms.extractKey.field": "Key"
  }'
