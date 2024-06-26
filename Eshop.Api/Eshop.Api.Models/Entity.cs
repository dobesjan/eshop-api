﻿using Eshop.Api.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models
{
	public class Entity : IEntity
	{
		[Key]
		public int Id { get ; set ; }

		public virtual object ToJson()
		{
			throw new NotImplementedException();
		}

		public virtual bool Validate()
		{
			return true;
		}
	}
}
