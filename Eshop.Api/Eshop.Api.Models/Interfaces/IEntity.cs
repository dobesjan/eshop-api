﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Interfaces
{
	public interface IEntity
	{
		int Id { get; set; }
		object ToJson();
	}
}
