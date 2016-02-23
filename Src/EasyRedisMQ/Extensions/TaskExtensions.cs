using System.Threading.Tasks;

namespace EasyRedisMQ.Extensions
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this Task taskToFireAndForget)
        {
            //The purpose of this is to explicitly state that we don't want to wait for the task to complete. 
            //Also, it will supress the warning that we would normally get if you don't await an async method
        }
    }
}
