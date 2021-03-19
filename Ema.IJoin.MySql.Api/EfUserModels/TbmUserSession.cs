﻿using System;
using System.Collections.Generic;

namespace Ema.IJoin.MySql.Api.EfUserModels
{
    public partial class TbmUserSession
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
        public string SessionId { get; set; }
        public string SessionName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CourseOwnerEmail { get; set; }
        public string CourseOwnerContactNo { get; set; }
        public string Venue { get; set; }
        public string Instructor { get; set; }
        public string CourseCreditHoursInit { get; set; }
        public string PassingCriteriaExceptionInit { get; set; }
        public string CourseCreditHours { get; set; }
        public string PassingCriteriaException { get; set; }
        public string IsCancel { get; set; }
        public string CoverPhotoName { get; set; }
        public string CoverPhotoUrl { get; set; }
        public DateTime Createdatetime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public virtual TbmUserSegment TbmUserSegment { get; set; }
        public virtual TbmUserSessionUser TbmUserSessionUser { get; set; }
    }
}
