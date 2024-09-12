using Soneta.Business;
using Soneta.Business.App;
using Soneta.Business.Db;
using Soneta.Core;
using Soneta.Core.DbTuples;
using Soneta.Towary;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testWorker;


[assembly:Worker(typeof(PrzykładWorkerDodawanie),typeof(Towary))]
namespace testWorker
{
    public class PrzykładWorkerDodawanie
    {

        [Context]
        public Context Context { get; set; }

        [Action("Dodaj z workera",Target = ActionTarget.ToolbarWithText,Mode = ActionMode.SingleSession)]
        public void Dodaj() 
        {
            Session Session = Context.Session;
            Operator op = Session.Login.Operator;

            using (var t = Context.Session.Logout(true)) 
            {
                var cm = CoreModule.GetInstance(Session);
                DbTupleDefinition def = cm.TuplesDefs.WgNazwa["Operators","Załączniki"];
                DbTuple tuple = def.CreateRow(op);
                cm.Tuples.AddRow(tuple);
                tuple.Fields["email"] = "adres@wp.pl";
                tuple.Fields["Data"] = Date.Today;
                t.CommitUI();

                //Attachment att = new Attachment(tuple, AttachmentType.Attachments);
                //att.AttachmentFile
            }

        }

    }
}
