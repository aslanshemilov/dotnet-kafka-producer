using System;
using System.Threading;
using Confluent.Kafka;
using System.Text.Json;
using Model.avro;
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;

namespace dotnet_kafka_producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = "192.168.1.211:9092"
                };

                //var producer = new ProducerBuilder<Null, string>(producerConfig).Build();


                var schemaRegistryConfig = new SchemaRegistryConfig
                {
                    Url = "192.168.1.211:8182"
                };

                /*
                var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
                var producer = new ProducerBuilder<Null, User2>(producerConfig)
                    .SetValueSerializer(new AvroSerializer<User2>(schemaRegistry))
                    .Build();*/

                while (true)
                {

                    // 1. produce just string
                    //var userName = Faker.Name.FirstName();
                    //Console.WriteLine("Program::Main: {0}", userName);
                    //producer.Produce("customer-topic-1", new Message<Null, string> {Value = userName});


                    // 2. produce json using avro
                    var user = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = Faker.Name.FirstName(),
                        LastName = Faker.Name.LastName()
                    };
                    Console.WriteLine("Program::Main: Creating User: {0}", user.FirstName);

                    var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
                    producer.Produce("customer-topic-1", new Message<Null, string> { Value = JsonSerializer.Serialize(user) });

                    /*
                    // 3. produce as avro using avro scema
                    var user = new User2
                    {
                        firstname = Faker.Name.FirstName(),
                        lastname = Faker.Name.LastName()

                    };

                    pbl(producer);

                    */



                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Program::Main: Exception: {0}", ex.Message);
            }
            finally
            {
                Console.WriteLine("Program::Main: finally block");
            }

            Console.ReadKey();
        }


        private static async void pbl(IProducer<Null, User2> producer)
        {
            try
            {
                var user = new User2
                {
                    firstname = Faker.Name.FirstName(),
                    lastname = Faker.Name.LastName()

                };             

                await producer.ProduceAsync("customer-topic-1", new Message<Null, User2> { Value = user });

                Console.WriteLine("Program::pbl: Creating User: {0}", user.firstname);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Program::pbl: Exception: {0}", ex.Message);
            }
            
        }


    } //end class
}
