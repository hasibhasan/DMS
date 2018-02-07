using DataModel.Models.DB;
using DataModel.Models.ViewModel;
using DMS.Models;
using DMS.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models.EntityManager
{
    public class AdminManager
    {
        private DDataEntities db = new DDataEntities();

        public List<BeneficiaryDetail> GetBenDetails()
        {

            try
            {
                var benDetails = db.BeneficiaryDetails.Take(1000).ToList();
                return benDetails;
            }
            catch
            {
                return null;
            }



        }

    }
}
