using System;
using System.Collections.Generic;
using System.Globalization;
using Ema.Ijoins.Api.EfModels;
using ExcelDataReader;
using System.Data;
using System.IO;

namespace Ema.Ijoins.Api.Helpers
{
  public static class Utility
  {

    public static List<TbKlcDataMaster> ReadExcel(string filePath, TbmKlcFileImport attachFiles)
    {
      List<TbKlcDataMaster> datas = new List<TbKlcDataMaster>();
      System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
      using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
      {
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
          int row = 0;
          while (reader.Read()) //Each ROW
          {
            if (row != 0)
              datas.Add(new TbKlcDataMaster
              {
                Id = row,
                FileId = attachFiles.Id,
                CourseType = reader.GetValue(0)?.ToString(),
                CourseId = reader.GetValue(1)?.ToString(),
                CourseName = reader.GetValue(2)?.ToString(),
                CourseNameTh = reader.GetValue(3)?.ToString(),
                SessionId = reader.GetValue(4)?.ToString(),
                SessionName = reader.GetValue(5)?.ToString(),
                SegmentNo = reader.GetValue(6)?.ToString(),
                SegmentName = reader.GetValue(7)?.ToString(),
                StartDate = reader.GetValue(8)?.ToString(),
                EndDate = reader.GetValue(9)?.ToString(),
                StartTime = reader.GetValue(10)?.ToString(),
                EndTime = reader.GetValue(11)?.ToString(),
                CourseOwnerEmail = reader.GetValue(12)?.ToString(),
                CourseOwnerContactNo = reader.GetValue(13)?.ToString(),
                Venue = reader.GetValue(14)?.ToString(),
                Instructor = reader.GetValue(15)?.ToString(),
                CourseCreditHours = reader.GetValue(16)?.ToString(),
                PassingCriteriaException = reader.GetValue(17)?.ToString(),
                UserCompany = reader.GetValue(18)?.ToString(),
                UserId = reader.GetValue(19)?.ToString(),
                RegistrationStatus = reader.GetValue(20)?.ToString(),
              });
            row++;
          }
        }
      }
      return datas;
    }

    public static List<TbKlcDataMaster> ValidateData(List<TbKlcDataMaster> tbKlcDatas)
    {
      List<TbKlcDataMaster> datasInvalid = new List<TbKlcDataMaster>();

      foreach (TbKlcDataMaster data in tbKlcDatas)
      {

        if (string.IsNullOrEmpty(data.CourseType) ||
        string.IsNullOrEmpty(data.CourseId) ||
        string.IsNullOrEmpty(data.CourseName) ||
        string.IsNullOrEmpty(data.SessionId) ||
        string.IsNullOrEmpty(data.SessionName) ||
        string.IsNullOrEmpty(data.SegmentNo) ||
        string.IsNullOrEmpty(data.StartDate) ||
        string.IsNullOrEmpty(data.EndDate) ||
        string.IsNullOrEmpty(data.StartTime) ||
        string.IsNullOrEmpty(data.EndTime) ||
        string.IsNullOrEmpty(data.CourseOwnerEmail) ||
        string.IsNullOrEmpty(data.UserId) ||
        string.IsNullOrEmpty(data.RegistrationStatus))
        {
          datasInvalid.Add(data);
        }

      }

      return datasInvalid;
    }
  }
}
