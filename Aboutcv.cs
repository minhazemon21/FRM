public ActionResult About()
        {
            int? UserId = SessionHelper.LoggedInUserId;
            var executives = officeExecutiveService.GetAll().Where(x => x.IsActive).ToList();
            ViewBag.ExecutiveList = executives;
            var executive = new OfficeExecutive() { Id = 0 };
            if (UserId.HasValue)
            {
                executive = executives.FirstOrDefault(x => x.Id == UserId);
            }
            ViewBag.Executive = executive;
            var param = new { UserId };
            ViewBag.ExecutiveProfieInfo = spService.GetDataWithParameter(param, "USP_Get_ExecutiveProfileInformation").Tables[0];
                

            return View();
        }
        public void ShowExecutiveDetails(int reportNo, string UserId)
        {
            var param = new { UserId = UserId };
            var data = spService.GetDataWithParameter(param, "USP_Get_ExecutiveProfileInformation").Tables[0];
            ReportHelper.ShowReport(data, "pdf", "rpt_ExecutiveProfileDetails.rpt", "ExecutiveProfileDetails");

        }