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

    public static List<TbKlcDataMaster> ReadExcel(string filePath)
    {
      List<TbKlcDataMaster> datas = new List<TbKlcDataMaster>();
      System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
      using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
      {
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {

          while (reader.Read()) //Each ROW
          {
            datas.Add(new TbKlcDataMaster
            {

              CourseId = reader.GetValue(1).ToString()

            });
          }
        }
      }
      return datas;
    }


  }
}
