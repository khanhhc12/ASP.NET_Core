using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld
{
    public static class SimpleSemaphoreSlim
    {
        //Instantiate a Singleton of the Semaphore with a value of 1. This means that only 1 thread can be granted access at a time.
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public static async void Run(DateTime date)
        {
            //Asynchronously wait to enter the Semaphore. If no-one has been granted access to the Semaphore, code execution will proceed, otherwise this thread waits here until the semaphore is released 
            await semaphoreSlim.WaitAsync();
            try
            {
                Console.WriteLine(date.ToString("HH:mm:ss.fff") + " - " + DateTime.Now.ToString("HH:mm:ss.fff"));
                await Task.Delay(1000);
            }
            finally
            {
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                semaphoreSlim.Release();
            }
        }

        public static void Test()
        {
            for (int i = 0; i < 10; i++)
            {
                Task.Run(() => { Run(DateTime.Now); });
            }
        }
    }
}