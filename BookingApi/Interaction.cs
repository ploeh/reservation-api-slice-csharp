﻿/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
namespace Ploeh.Samples.BookingApi
{
    public class Interaction
    {
        public string Time { get; set; }
        public string Operation { get; set; }
        public object Input { get; set; }
        public object Output { get; set; }
    }
}