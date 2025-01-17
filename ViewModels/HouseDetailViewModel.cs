﻿using BTL_WEB_NC.Models;

public class HouseDetailViewModel
{
    public House? House { get; set; }
    public List<ImageCategory>? Images { get; set; }
    public BookingCalender? BookingCalender { get; set; }
    public bool IsScheduled { get; set; }
}