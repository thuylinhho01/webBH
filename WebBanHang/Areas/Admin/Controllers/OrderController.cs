using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var items = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var items = db.OrderDetails.Where(x=>x.OrderId == id).ToList();
            return PartialView(items);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.TypePayment = trangthai;
                db.Entry(item).Property(x => x.TypePayment).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public List<ExcelModel> GetOrderDetailForExcel(int orderid)
        {
            using (db)
            {
                var lsDetail = db.OrderDetails.Join(db.Products, x => x.ProductId,
                                                        y => y.Id,
                                                        (x, y) => new { detail = x, product = y })
                                                        .Where(x => x.detail.OrderId == orderid)
                                                        .Select(x => new ExcelModel
                                                        {
                                                            OrderId = x.detail.OrderId,
                                                            ProductName = x.product.Title,
                                                            Quantity = x.detail.Quantity,
                                                            Price = x.detail.Price,
                                                            TotalPrice = x.detail.Price * x.detail.Quantity
                                                           
                                                        }).ToList();

                return lsDetail;
            }
        }

        //public List<ExcelModel> GetOrderDetailForExcelName(int id)
        //{
        //    using (db)
        //    {
        //        var lsDetailName = db.OrderDetails.Join(db.Orders, x => x.OrderId,
        //                                                h => h.Id,
        //                                                (x, h) => new { detail = x, order = h })
        //                                                .Where(x => x.detail.OrderId == id)
        //                                                .Select(h => new ExcelModel
        //                                                {
        //                                                    Code = h.order.Code,
        //                                                    CustomerName = h.order.CustomerName,
        //                                                    Phone = h.order.Phone,
        //                                                    Address = h.order.Address

        //                                                }).ToList();
        //        return lsDetailName;
        //    }
        //}
        public ActionResult Excel(int id)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("OrderDetails");


            ws.Range("A1:Z300").Style.Font.FontName = "Times new roman"; //Font chữ
            ws.Range("A1:B3").Style.Font.FontSize = 10;
            ws.Range("A1:B3").Style.Font.Bold = true;
            ws.Range("A1:B3").Style.Font.FontColor = XLColor.BabyBlue; //Màu xanh da trời


            //var lsName = GetOrderDetailForExcelName(id);
            //for (int j = 0; j < lsName.Count; j++)
            //{
            //    ws.Cell("C7").Value = lsName[j].Code;
            //}



            ws.Range("A1:B1").Row(1).Merge();
            ws.Range("A1:B1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("A1:B1").Value = "LinhShop";
            ws.Range("A2:B2").Row(1).Merge();
            ws.Range("A2:B2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("A2:B2").Value = "Nghệ An";
            ws.Range("A3:B3").Row(1).Merge();
            ws.Range("A3:B3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("A3:B3").Value = "Điện thoại: (04)38526419";
            ws.Range("C5:E5").Style.Font.FontSize = 16;
            ws.Range("C5:E5").Style.Font.Bold = true;
            ws.Range("C5:E5").Style.Font.FontColor = XLColor.Red; //Màu đỏ
            ws.Range("C5:E5").Row(1).Merge();
            ws.Range("C5:E5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("C5:E5").Value = "HÓA ĐƠN BÁN";
            // Biểu diễn thông tin chung của hóa đơn bán
            //sql = "SELECT MaHD, NgayBan, TongTien,MaNV FROM HDBAN WHERE MaHD = '" + textBoxMahoadon.Text + "' ";
            //tblThongtinHD = Function.GetDataToTable(sql);
            ws.Range("B7:C9").Style.Font.FontSize = 12;
            ws.Range("B7:B7").Value = "Mã hóa đơn:";
            ws.Range("C7:E7").Row(1).Merge();
            ws.Range("B8:C10").Style.Font.FontSize = 12;
            ws.Range("B8:B8").Value = "Tên khách hàng: ";
            ws.Range("C8:E8").Row(1).Merge();

            ws.Columns(1,300).AdjustToContents();
            ws.Range("A11:E11").Style.Font.Bold = true;

      
            
            var col11 = ws.Column("A");
            col11.Width = 15;
            //ws.Cell("A11:E11").Style.Fill.BackgroundColor = XLColor.PrussianBlue;
            //ws.Cell("A11:E11").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            //ws.Cell("A11:E11").Style.Border.RightBorderColor = XLColor.Black;
            ws.Range("A11:E300").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A11").Value = "Mã sản phẩm";
            ws.Cell("B11").Value = "Tên sản phẩm";
            ws.Cell("C11").Value = "Số lượng";
            ws.Cell("D11").Value = "Giá";
            ws.Cell("E11").Value = "Thành tiền";

            var ls = GetOrderDetailForExcel(id);
            //var lsName = GetOrderDetailForExcel(orderid);
            int row = 12;
            for(int i = 0; i < ls.Count; i++)
            {
               
                ws.Cell("A" + row).Value = ls[i].OrderId;
                ws.Cell("B" + row).Value = ls[i].ProductName;
                ws.Cell("C" + row).Value = ls[i].Quantity;
                ws.Cell("D" + row).Value = ls[i].Price;
                ws.Cell("E" + row).Value = ls[i].TotalPrice;
                row++;
            }
            


            string nameFile = "Export_" + DateTime.Now.Ticks + ".xlsx";
            string pathFile = Server.MapPath("~/Resource/ExportExcel/" + nameFile);
           
            wb.SaveAs(pathFile);
            return Json(nameFile, JsonRequestBehavior.AllowGet);

        }
       

    }
}