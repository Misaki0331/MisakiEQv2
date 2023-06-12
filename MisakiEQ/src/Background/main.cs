

namespace MisakiEQ.Background
{
    internal class APIs
    {
        static APIs? singleton = null;
        public static APIs Instance
        {
            get
            {
                singleton ??= new APIs();
                return singleton;
            }
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
            await EEW.AbortAndWait();
            await EQInfo.AbortAndWait();
            await KyoshinAPI.AbortAndWait();
            await Jalert.AbortAndWait();
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
