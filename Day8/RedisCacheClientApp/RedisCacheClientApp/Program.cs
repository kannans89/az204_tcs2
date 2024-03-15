
using StackExchange.Redis;

string connectionString = "day8redisstored.redis.cache.windows.net:6380,password=NzAaIpIsuFyCOpB4I164elp2MQqHYGbbYAzCaOB51g0=,ssl=True,abortConnect=False";

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);



void GetCacheData()
{
    IDatabase database = redis.GetDatabase();
    if (database.KeyExists("message1")) //cache hit
        Console.WriteLine(database.StringGet("message1"));
    else //load it from service and get data
        Console.WriteLine("key does not exist"); // cache miss

}
void SetCacheData(string key, string value)
{
    IDatabase database = redis.GetDatabase();

    database.StringSet(key, value);

    Console.WriteLine("Cache data set");
}
//GetCacheData();
SetCacheData("message2", "in day8");
Console.WriteLine("End");
