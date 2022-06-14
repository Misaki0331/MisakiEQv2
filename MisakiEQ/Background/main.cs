

namespace MisakiEQ.Background
{
    internal class APIs
    {
        static APIs? singleton = null;
        public static APIs GetInstance()
        {
            if (singleton == null)
            {
                singleton = new APIs();
            }
            return singleton;
        }
        private APIs()
        {

        }
        public API._EEW EEW =new ();
        public API._EQInfo EQInfo = new();
        public API.KyoshinAPI.KyoshinAPI KyoshinAPI=new();
        public void Init()
        {
            EEW.Init();
            EQInfo.Init();
        }
        public async Task Abort()
        {
            var a = EEW.AbortAndWait();
            var b = EQInfo.AbortAndWait();
            var c = KyoshinAPI.AbortAndWait();
            await a;
            await b;
            await c;
        }
        public void Run()
        {
            EEW.RunThread();
            EQInfo.RunThread();
            KyoshinAPI.RunThread();
        }
    }
}
