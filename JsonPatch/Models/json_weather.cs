using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace JsonPatch.Models
{
    public class json_weather :  DbContext
    {
        public json_weather()
        {
        }

        public json_weather(DbContextOptions<json_weather> options)
            : base(options)
        {
        }

        public virtual DbSet<weather> climate { get; set; }

    }
}
