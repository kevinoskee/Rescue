using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Threading;
using System.Threading.Tasks;

namespace Rescue.Droid
{
    [Service]
    public class PowerButtonService : Service
    {
        CancellationTokenSource _cts;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            var t = new Java.Lang.Thread(() =>
            {
                _cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    try
                    {
                        var counter = new ScreenReceiver();
                        //counter.ExecutePost(_cts.Token).Wait();
                    }
                    catch (Android.Accounts.OperationCanceledException)
                    {
                    }
                    finally
                    {
                        if (_cts.IsCancellationRequested)
                        {

                        }
                    }

                }, _cts.Token);
            }
            );
         t.Start();
            return StartCommandResult.RedeliverIntent;
        }
    }
}