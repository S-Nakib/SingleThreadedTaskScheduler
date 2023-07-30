# SingleThreadedTaskScheduler

The .NET default TaskScheduler <b>doesn't</b> guarantee that two tasks of the same Request will get executed on the same Thread. 
</br>
To know more read [Internal Mechanisms of Tasks in .NET](https://medium.com/net-under-the-hood/internal-mechanisms-of-tasks-in-net-ef461956d4a7)


This behaviour of .NET can create issues in some situations. Sometimes we want some of our tasks executed on the Same Thread. 

For example, Say we are using some library that doesn't support getting called from multiple Threads. 

To solve this problem, we are creating a Custom TaskScheduler that will execute tasks on the same thread. 


The project is not finished yet. 

### Todos
1. Maybe we need to make it IDisposable. 
2. We need to maintain a threadpool. 
3. Test and brainstorm if there is any other issues. 
