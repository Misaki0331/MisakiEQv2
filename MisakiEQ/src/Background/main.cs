

namespace MisakiEQ.Background
{
    internal class APIs
    {
        static APIs? singleton = null;
        public static APIs GetInstance()
        {
            singleton ??= new APIs();
            return singleton;
        }
        private APIs()
        {

        }
        public API._EEW EEW =new ();
        public API._EQInfo EQInfo = new();
        public API.KyoshinAPI.KyoshinAPI KyoshinAPI=new();
        public API.JAlert Jalert = new();
        public void Init()
        {
            EEW.Init();
            EQInfo.Init();
            Jalert.Init();
        }
        public async Task Abort()
        {
            var a = EEW.AbortAndWait();
            var b = EQInfo.AbortAndWait();
            var c = KyoshinAPI.AbortAndWait();
            var d = Jalert.AbortAndWait();
            await a;
            await b;
            await c;
            await d;
        }
        public void Run()
        {
            EEW.RunThread();
            EQInfo.RunThread();
            KyoshinAPI.RunThread();
            Jalert.RunThread();
        }
    }
}
