using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleThreadedTaskScheduler.SingleThreadedTaskScheduler;

public class SingleThreadedTaskScheduler: TaskScheduler
{
    private readonly Thread _thread;
    private readonly ManualResetEvent _mre;
    private readonly ConcurrentStack<Task> _tasks;

    public override int MaximumConcurrencyLevel => 1;

    public SingleThreadedTaskScheduler()
    {
        _mre = new(false);
        _tasks = new();
        _thread = new Thread(TaskExecutor);
        _thread.IsBackground = true;

        _thread.Start();
        Console.WriteLine($"ThreadId: {_thread.ManagedThreadId}");
    }
    protected override IEnumerable<Task> GetScheduledTasks()
    {
        return new List<Task>(_tasks);
    }

    protected override void QueueTask(Task task)
    {
        lock (_tasks)
        {
            _tasks.Push(task);
            _mre.Set();
        }
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return false;
    }

    private void TaskExecutor()
    {
        while (true)
        {
            _mre.WaitOne();

            _tasks.TryPop(out var task);
            if (task is not null)
                TryExecuteTask(task);

            lock (_tasks)
            {
                if (_tasks.IsEmpty)
                {
                    _mre.Reset();
                }
            }
        }
    }
}