﻿using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserAddress
{
    public class ViewAddressDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressType { get; set; }
        public bool IsDefault { get; set; }
        public string FullAddress { get; set; }
        public int ProviceId { get; set; }
        public int DistrictId { get; set; }
        public string WardCode { get; set; }
    }
}
