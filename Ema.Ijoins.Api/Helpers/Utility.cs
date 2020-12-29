using System;
using System.Collections.Generic;
using System.Globalization;
using Ema.Ijoins.Api.EfModels;
using ExcelDataReader;
using System.Data;
using System.IO;
using OfficeOpenXml;

namespace Ema.Ijoins.Api.Helpers
{
  public static class Utility
  {
    public static List<TbKlcDataMaster> ReadExcelEPPlus(string filePath, TbmKlcFileImport attachFiles)
    {
      List<TbKlcDataMaster> datas = new List<TbKlcDataMaster>();

      FileInfo existingFile = new FileInfo(filePath);
      using (ExcelPackage package = new ExcelPackage(existingFile))
      {
        //Get the first worksheet in the workbook
        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        int totalRows = worksheet.Dimension.Rows;

        CultureInfo enUS = new CultureInfo("en-US");

        for (int row = 2; row <= totalRows; row++)
        {

          DateTime StartDate = (DateTime)worksheet.Cells[row, 9].Value;
          DateTime EndDate = (DateTime)worksheet.Cells[row, 10].Value;

          DateTime StartTime = (DateTime)worksheet.Cells[row, 11].Value;
          DateTime EndTime = (DateTime)worksheet.Cells[row, 12].Value;

          DateTime.TryParseExact(StartDate.ToString("yyyyMMdd") + " " + StartTime.ToString("HH:mm:ss"), "yyyyMMdd HH:mm:ss", enUS, DateTimeStyles.None, out DateTime StartDateTime);
          DateTime.TryParseExact(EndDate.ToString("yyyyMMdd") + " " + EndTime.ToString("HH:mm:ss"), "yyyyMMdd HH:mm:ss", enUS, DateTimeStyles.None, out DateTime EndDateTime);

          datas.Add(new TbKlcDataMaster
          {
            FileId = attachFiles.Id,
            CourseType = worksheet.Cells[row, 1].Value?.ToString(),
            CourseId = worksheet.Cells[row, 2].Value?.ToString(),
            CourseName = worksheet.Cells[row, 3].Value?.ToString(),
            CourseNameTh = worksheet.Cells[row, 4].Value?.ToString(),
            SessionId = worksheet.Cells[row, 5].Value?.ToString(),
            SessionName = worksheet.Cells[row, 6].Value?.ToString(),
            SegmentNo = worksheet.Cells[row, 7].Value?.ToString(),
            SegmentName = worksheet.Cells[row, 8].Value?.ToString(),
            StartDate = worksheet.Cells[row, 9].Value?.ToString(),
            EndDate = worksheet.Cells[row, 10].Value?.ToString(),
            StartTime = worksheet.Cells[row, 11].Value?.ToString(),
            EndTime = worksheet.Cells[row, 12].Value?.ToString(),
            StartDateTime = StartDateTime,
            EndDateTime = EndDateTime,
            CourseOwnerEmail = worksheet.Cells[row, 13].Value?.ToString(),
            CourseOwnerContactNo = worksheet.Cells[row, 14].Value?.ToString(),
            Venue = worksheet.Cells[row, 15].Value?.ToString(),
            Instructor = worksheet.Cells[row, 16].Value?.ToString(),
            CourseCreditHours = worksheet.Cells[row, 17].Value?.ToString(),
            PassingCriteriaException = worksheet.Cells[row, 18].Value?.ToString(),
            UserCompany = worksheet.Cells[row, 19].Value?.ToString(),
            UserId = worksheet.Cells[row, 20].Value?.ToString().PadLeft(8, '0'),
            RegistrationStatus = worksheet.Cells[row, 21].Value?.ToString(),
          });
        }

      }

      return datas;
    }
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
          datasInvalid.Add(new TbKlcDataMaster
          {
            Id = data.Id,
            FileId = data.FileId,
            CourseType = data.CourseType,
            CourseId = data.CourseId,
            CourseName = data.CourseName,
            CourseNameTh = data.CourseNameTh,
            SessionId = data.SessionId,
            SessionName = data.SessionName,
            SegmentNo = data.SegmentNo,
            SegmentName = data.SegmentName,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            StartTime = data.StartTime,
            EndTime = data.EndTime,
            CourseOwnerEmail = data.CourseOwnerEmail,
            CourseOwnerContactNo = data.CourseOwnerContactNo,
            Venue = data.Venue,
            Instructor = data.Instructor,
            CourseCreditHours = data.CourseCreditHours,
            PassingCriteriaException = data.PassingCriteriaException,
            UserCompany = data.UserCompany,
            UserId = data.UserId,
            RegistrationStatus = data.RegistrationStatus,
          });
        }

      }
      return datasInvalid;
    }

    public static List<TbKlcDataMasterHi> MoveDataToHis(List<TbKlcDataMaster> tbKlcDatas)
    {
      List<TbKlcDataMasterHi> dataHis = new List<TbKlcDataMasterHi>();

      foreach (TbKlcDataMaster data in tbKlcDatas)
      {
        dataHis.Add(new TbKlcDataMasterHi
        {
          Id = data.Id,
          FileId = data.FileId,
          CourseType = data.CourseType,
          CourseId = data.CourseId,
          CourseName = data.CourseName,
          CourseNameTh = data.CourseNameTh,
          SessionId = data.SessionId,
          SessionName = data.SessionName,
          SegmentNo = data.SegmentNo,
          SegmentName = data.SegmentName,
          StartDate = data.StartDate,
          EndDate = data.EndDate,
          StartTime = data.StartTime,
          EndTime = data.EndTime,
          CourseOwnerEmail = data.CourseOwnerEmail,
          CourseOwnerContactNo = data.CourseOwnerContactNo,
          Venue = data.Venue,
          Instructor = data.Instructor,
          CourseCreditHours = data.CourseCreditHours,
          PassingCriteriaException = data.PassingCriteriaException,
          UserCompany = data.UserCompany,
          UserId = data.UserId,
          RegistrationStatus = data.RegistrationStatus,
        });
      }
      return dataHis;
    }

    public static List<TbtIjoinScanQrHi> MoveDataIJoinToHis(List<TbtIjoinScanQr> lists)
    {
      List<TbtIjoinScanQrHi> dataHis = new List<TbtIjoinScanQrHi>();

      foreach (TbtIjoinScanQr data in lists)
      {
        dataHis.Add(new TbtIjoinScanQrHi
        {
          Id = data.Id,
          FileId = data.FileId,
          CourseTypeId = data.CourseTypeId,
          CourseId = data.CourseId,
          SessionId = data.SessionId,
          StartDateTime = data.StartDateTime,
          EndDateTime = data.EndDateTime,
          UserId = data.UserId,
          RegistrationStatus = data.RegistrationStatus,
          CheckInDateTime = data.CheckInDateTime,
          CheckOutDateTime = data.CheckOutDateTime,
          Createdatetime = data.Createdatetime,
          Updatedatetime = data.Updatedatetime,
          UpdateBy = data.UpdateBy,
        });
      }
      return dataHis;
    }

  }
}
