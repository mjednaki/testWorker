using Soneta.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testWorker;
using Soneta.Towary;
using Soneta.Types;


[assembly:Worker(typeof(PrzykladWorker), typeof(Towary))]
namespace testWorker
{
    public class PrzykladWorker
    {

        [Context]
        public Towar[] Towary { get; set; }

        [Context]
        public Prms Prms { get; set; }

        [Action("Nasza metoda",Target = ActionTarget.ToolbarWithText | ActionTarget.Menu,Mode = ActionMode.SingleSession, Icon = ActionIcon.Wizard )]
        public void NaszaMetoda() 
        {

            foreach (var t in Towary)
            {
                using (ITransaction trans = t.Session.Logout(true))
                {
                    t.Nazwa = t.Nazwa + Prms.Opis;

                    trans.CommitUI();
                }
            }
        }

        

    }


    public class Prms : ContextBase
    {
        public Prms(Context cx) : base(cx) 
        { 
        }


        public Date Data { get; set; }
        public string Opis { get; set; }

    }

}
