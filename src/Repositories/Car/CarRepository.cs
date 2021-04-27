using CarStocks.Common;
using CarStocks.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CarStocks.Repositories
{
    public class CarRepository : BaseRepository, ICarRepository
    {
        public CarRepository(IConfiguration config) : base(config)
        {
        }

        public void Delete(Car entity)
        {
            var sql = "DELETE FROM Car WHERE Id = @id";
            using var db = this.GetDbconnection();

            db.Execute(sql, new {id = entity.Id});
        }

        public Car Get(int id)
        {
            using var db = this.GetDbconnection();
            
            return db.Query<Car>("SELECT * FROM CAR WHERE Id=@id", new {id = id}).FirstOrDefault();
        }

        public Car Insert(Car entity)
        {
            var sql = "INSERT INTO Car (DealerId,Make,Model,Year,Stock) VALUES (@DealerId,@Make,@Model,@Year,@Stock); ";

            using var db = this.GetDbconnection();

            entity.Id  = db.ExecuteScalar<int>(sql, entity);

            return entity;             
        } 

        public List<Car> GetAll(int dealer, string make, string model)
        {
            var builder = new SqlBuilder();

            var template = builder.AddTemplate("SELECT * FROM Car /**where**/");
            builder.Where("DealerId = @dealerId", new {dealerId = dealer});
            if(!string.IsNullOrEmpty(make)) {
                builder.Where("Make LIKE '%' || @make || '%'", new { make });
            }
            if(!string.IsNullOrEmpty(model)) {
                builder.Where("Model LIKE '%' || @model || '%'", new { model });
            }

            using var db = this.GetDbconnection();

            return db.Query<Car>(template.RawSql, template.Parameters).ToList();
        }

        public Car Update(Car entity)
        {
            var sql = "UPDATE Car SET Id = @Id,DealerId = @DealerId,Make = @Make,Model = @Model,Year = @Year,Stock = @Stock WHERE Id = @Id;";

            using var db = this.GetDbconnection();

            db.Execute(sql, entity);

            return entity;
        }
    }
}
