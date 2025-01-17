﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using BTL_WEB_NC.Models;
using BTL_WEB_NC.ViewModels;
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BTL_WEB_NC.Data;

namespace BTL_WEB_NC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetHouseData()
        {
            var houses = _context.Houses.Where(h => h.HouseStatus == "Còn phòng").ToList();
            var houseIds = houses.Select(h => h.Id).ToList();
            var images = _context.ImageCategories.Where(ic => houseIds.Contains(ic.HouseId)).ToList();
            var viewHouseModel = new HouseViewModel
            {
                Houses = houses,
                Images = images
            };

            return Json(viewHouseModel);
        }
        public JsonResult Search(string keyword)
        {
            // Phân tách chuỗi thành các giá trị riêng lẻ
            var keywordArray = keyword.Split(',');

            // Khởi tạo các biến để lưu trữ các giá trị từ chuỗi
            var priceString = "";
            var address = "";
            var acreage = "";

            // Lặp qua từng phần tử trong mảng chuỗi
            foreach (var item in keywordArray)
            {
                // Kiểm tra nếu phần tử chứa từ "triệu"
                if (item.Contains("triệu"))
                {
                    // Lấy giá trị giá từ chuỗi
                    priceString = item.Replace(" triệu", "").Trim();
                }
                // Kiểm tra nếu phần tử chứa từ "m2"
                else if (item.Contains("m2"))
                {
                    // Lấy giá trị diện tích từ chuỗi
                    acreage = item.Replace("m2", "").Trim();
                }
                // Nếu không phải là giá trị giá hoặc diện tích, giả sử là địa chỉ
                else
                {
                    // Lấy giá trị địa chỉ từ chuỗi
                    address += item.Trim() + " ";
                }
            }

            // Tìm kiếm trong cơ sở dữ liệu
            var housesSearch = _context.Houses.Where(p =>
                (string.IsNullOrEmpty(priceString) || p.Price.Contains(priceString)) &&
                (string.IsNullOrEmpty(address) || p.Address.Contains(address.Trim())) &&
                (string.IsNullOrEmpty(acreage) || p.Acreage == int.Parse(acreage))
            ).ToList();
            var images = _context.ImageCategories.ToList();
            var viewHouseModel = new HouseViewModel
            {
                Houses = housesSearch,
                Images = images
            };
            // Trả về kết quả
            if (housesSearch.Count == 0)
            {
                return Json(new { error = "Không tìm thấy dữ liệu!" });
            }
            else
            {
                return Json(viewHouseModel);
            }
        }



        public JsonResult GetLocations()
        {
            var locations = _context.Locations.ToList();
            var houses = _context.Houses.ToList();

            var viewModel = new ViewAddress
            {
                Locations = locations,
                Houses = houses
            };

            return Json(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
