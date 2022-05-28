//using Yitter.IdGenerator;

//namespace Miracle.Common.Tools;
//public class SnowFlakeService : AppModule
//{
//    public override void ConfigureServices(ConfigureServicesContext context)
//    {
//        var options = new IdGeneratorOptions()
//        {
//            Method = 1, // 1-漂移算法，2-传统算法
//            WorkerId = 1,
//            WorkerIdBitLength = 6,
//            SeqBitLength = 6,
//            TopOverCostCount = 2000,
//            MinSeqNumber = 1,
//            MaxSeqNumber = 200,
//            BaseTime = DateTime.Now.AddYears(-10),
//        };
//        // 保存参数（必须的操作，否则以上设置都不能生效）：
//        YitIdHelper.SetIdGenerator(options);
//        _ = context.Services.AddSingleton<YitIdHelper>();
//    }
//}