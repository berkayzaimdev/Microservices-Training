using System.Diagnostics;
using NLog;

Logger logger = LogManager.GetCurrentClassLogger();
Trace.CorrelationManager.ActivityId = Guid.NewGuid();

Work1();

void Work1()
{
    Console.WriteLine("work1 tetiklendi!");
    logger.Debug("work1 tetiklendi!");
    Work2();
}

void Work2()
{
    Console.WriteLine("work2 tetiklendi!");
    logger.Debug("work2 tetiklendi!");
    try
    {
        throw new Exception();
    }
    catch (Exception e)
    {
        Console.WriteLine("exception from work2");
        logger.Error(e);
    }
    Work3();
}

void Work3()
{
    Console.WriteLine("work3 tetiklendi!");
    logger.Debug("work3 tetiklendi!");
}