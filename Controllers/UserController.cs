﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using BTL_WEB_NC.Models;
using BTL_WEB_NC.ViewModels;
using static BTL_WEB_NC.Controllers.AdminController;
using BTL_WEB_NC.Data;

namespace BTL_WEB_NC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationContext _context;
        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult UserManage()
        {
            if (HttpContext.Session.GetString("Role") == "role1")
            {
                return View();
            }
            else
            {
                return RedirectToAction("UserInfo", "User");
            }
        }
        public IActionResult UserInfo()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var email = HttpContext.Session.GetString("Username");
                User user = new User();
                user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult UserInfoHouseBooking()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                User user = new User();
                List<House> houses = new List<House>();
                List<BookingCalender> bookingCalenders = new List<BookingCalender>();
                List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();
                var email = HttpContext.Session.GetString("Username");
                user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    bookingCalenders = _context.BookingCalenders.Where(b => b.CustomerId == user.Id).ToList();
                    foreach (var bookingCalender in bookingCalenders)
                    {
                        House house = _context.Houses.FirstOrDefault(h => h.Id == bookingCalender.HouseId);
                        if (house != null)
                        {
                            BookingViewModel bookingViewModel = new BookingViewModel();
                            bookingViewModel.bookingId = bookingCalender.Id;
                            bookingViewModel.housePrice = house.Price;
                            bookingViewModel.houseName = house.HouseTitle;
                            bookingViewModel.houseTittle = bookingCalender.Note;
                            bookingViewModel.houseCreatedAt = (DateTime)bookingCalender.CreatedAt;
                            bookingViewModel.houseAddress = house.Address;
                            bookingViewModels.Add(bookingViewModel);
                        }
                    }
                    UserInfoHouseBookingViewModel userInfoHouseBookingViewModel = new UserInfoHouseBookingViewModel
                    {
                        user = user,
                        bookingList = bookingViewModels
                    };
                    return View(userInfoHouseBookingViewModel);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult DeleteBooking(Guid bookingId)
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var email = HttpContext.Session.GetString("Username");
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var booking = _context.BookingCalenders.FirstOrDefault(b => b.Id == bookingId && b.CustomerId == user.Id);
                    if (booking != null)
                    {
                        _context.BookingCalenders.Remove(booking);
                        _context.SaveChanges();
                        return RedirectToAction("UserInfoHouseBooking");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //DataTable=====================================================================================
        [HttpGet]
        public string GetListUser()
        {
            List<User> users = _context.Users.ToList();
            foreach (var user in users)
            {
                var roleName = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.Name;
                user.RoleId = roleName ?? "Không xác định";
            }
            var value = JsonConvert.SerializeObject(new { data = users });

            return value;
        }

        // Cập nhật========================================================================================
        [HttpPost]
        public async Task<int> CapNhat([FromBody] User userObject)
        {
            var result = -1;

            try
            {
                _context.Users.Update(userObject);
                await _context.SaveChangesAsync();
                result = 1;
            }
            catch (DbUpdateConcurrencyException)
            {
                result = 0;
            }
            return result;
        }

        //Chi tiết=====================================================================================
        [HttpGet]
        public JsonResult ChiTiet(Guid userId)
        {
            var user = (from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.Id
                        where u.Id == userId
                        select new
                        {
                            name = u.Name,
                            phoneNumber = u.PhoneNumber,
                            email = u.Email,
                            password = u.Password,
                            gender = u.Gender,

                            roleId = r.Id

                        }).ToList();
            return Json(user);
        }

        // Xóa=====================================================================================================
        [HttpPost]

        public async Task<int> DeleteConfirmed(Guid userId)
        {
            var result = -1;
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                result = 1;
            }
            else
            {
                result = 0;
            }

            return result;
        }


        //DataTable=====================================================================================
        [HttpPost]
        public JsonResult getDataTablesWithUserId(Guid userId)
        {
            var listBooking = _context.BookingCalenders.Where(bc => bc.CustomerId == userId).ToList();

            List<object> objectArray = new List<object>();
            foreach (var booking in listBooking)
            {
                if (!string.IsNullOrEmpty(booking.HouseId.ToString()))
                {
                    var houseQuery = (from h in _context.Houses
                                      join l in _context.Locations on h.OfLocationId equals l.Id
                                      where h.Id == booking.HouseId
                                      select new
                                      {
                                          id = h.Id,
                                          idBooking = booking.Id,
                                          houseType = h.HouseType,
                                          houseStatus = h.HouseStatus,
                                          price = h.Price,
                                          address = h.Address,
                                          locationName = l.Name
                                      }).FirstOrDefault();

                    if (houseQuery != null)
                    {
                        objectArray.Add(houseQuery);
                    }
                }
            }
            return Json(new { data = objectArray });
        }

    }
}
