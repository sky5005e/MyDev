using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class EmailRepository : RepositoryBase
    {
        // Get Email Templates by Name
        public INC_EmailTemplate GetEmailTemplatesByName(String TempName)
        {
            return db.INC_EmailTemplates.Where(s => s.sTemplateName == TempName).FirstOrDefault();
        }
    }
}
