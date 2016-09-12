using System.Threading;
using System.Collections;

public class ThreadJob
{
    private bool _IsDone = false;
    private object _Handle = new object();
    private Thread _Thread = null;
    public bool IsDone
    {
        get
        {
            bool tmp;
            lock (_Handle)
            {
                tmp = _IsDone;
            }
            return tmp;
        }
        set
        {
            lock (_Handle)
            {
                _IsDone = value;
            }
        }
    }

    public virtual void Start()
    {
        _Thread = new Thread(Run);
        _Thread.Start();
    }

    public virtual void Abort()
    {
        _Thread.Abort();
    }

    protected virtual void ThreadFunction() { }

    protected virtual void OnFinished() { }

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinished();
            return true;
        }
        return false;
    }

    public IEnumerator WaitFor()
    {
        while (!Update())
        {
            yield return null;
        }
    }

    private void Run()
    {
        IsDone = false;
        ThreadFunction();
        IsDone = true;
    }
}
