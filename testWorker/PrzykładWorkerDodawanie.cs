using Soneta.Business;
using Soneta.Business.App;
using Soneta.Business.Db;
using Soneta.Core;
using Soneta.Core.DbTuples;
using Soneta.Towary;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.IO;
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
            DbTuple tuple = null;

            using (Session newSession = Session.Login.CreateSession(false, false)) 
            {
                using (var tr = newSession.Logout(true))
                {
                    var cm = CoreModule.GetInstance(newSession);

                    DbTupleDefinition def = cm.TuplesDefs.WgNazwa["Operators", "Załączniki"];
                    tuple = def.CreateRow(op);
                    cm.Tuples.AddRow(tuple);
                    tuple.Fields["email"] = "adres@wp.pl";
                    tuple.Fields["Data"] = Date.Today;
                    tr.CommitUI();
                }
                newSession.Save();
            }


            using (var t = Session.Logout(true))
            {
                DbTuple tupel = CoreModule.GetInstance(Session).Tuples[tuple.Guid] ;
                Attachment att = new Attachment(tupel, AttachmentType.Attachments);
                BusinessModule.GetInstance(Session).Attachments.AddRow(att);
                att.Name = "Test";

                Stream fs = File.OpenRead(@"C:\enovaNET\1.pdf");

                att.LoadFromStream(fs);

                t.CommitUI();
            }

        }

    }
}
