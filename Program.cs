using System.Data;
using System.Data.SqlClient;
using BaltaDataAcess.Models;
using Dapper;

const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";

using (var connection = new SqlConnection(connectionString))
{
    // CreateCategory(connection);
    // CreateManyCategory(connection);
    // UpdateCategory(connection);
    // DeleteCategory(connection);
    // DeleteManyCategory(connection);
    // ListCategories(connection);
    // ExecuteProcedure(connection);
    // ExecuteReadProcedure(connection);
    // ExecuteScalar(connection);
    // ReadView(connection);
    // OneToOne(connection);
    // OneToMany(connection);
    // QueryMultiple(connection);
    // SelectIn(connection);
    // Like(connection, "web");
    Transaction(connection);
}

static void ListCategories(SqlConnection connection)
{
    var categories = connection.Query<BaltaDataAcess.Models.Category>("SELECT [Id], [Title] FROM [Category]");
    foreach (var item in categories)
    {
        Console.WriteLine($"{item.Id} - {item.Title}");
    }
}

static void CreateCategory(SqlConnection connection)
{
    var category = new Category();
    category.Id = Guid.NewGuid();
    category.Title = "Amazon AWS";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços do AWS";
    category.Order = 8;
    category.Summary = "AWS Cloud";
    category.Featured = false;

    var insertSql = @"INSERT INTO 
            [Category] 
        VALUES(
            @Id,
            @Title,
            @Url,
            @Summary, 
            @Order, 
            @Description, 
            @Featured)";

    var rows = connection.Execute(insertSql, new
    {
        category.Id,
        category.Title,
        category.Url,
        category.Summary,
        category.Order,
        category.Description,
        category.Featured
    });
    Console.WriteLine($"{rows} linhas inseridas");
}

static void UpdateCategory(SqlConnection connection)
{
    var updatequery = "UPDATE [Category] SET [Title]=@Title WHERE [Id]=@Id";
    var rows = connection.Execute(updatequery, new
    {
        id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
        title = "Frontend 2021"
    });

    Console.WriteLine($"{rows} registros atualizadas");
}

static void DeleteCategory(SqlConnection connection)
{
    var deletequery = "DELETE [Category] WHERE [Id]=@Id";
    var rows = connection.Execute(deletequery, new
    {
        id = new Guid("a7548505-cb71-4b01-8f88-1071e5b3c4bc")
    });

    Console.WriteLine($"{rows} registros deletados");
}

static void DeleteManyCategory(SqlConnection connection)
{
    var deletequery = "DELETE [Category] WHERE [Id]=@Id";
    var rows = connection.Execute(deletequery, new[]{
    new
    {
        id = new Guid("3b279212-554a-4cd7-819a-32954217b594")
    },
    new
    {
        id = new Guid("85ead8e8-0eed-4430-9f1c-90ab8a6a198b")
    },
    new
    {
        id = new Guid("4224a135-0f1c-4c18-8e1b-c9e61a678f84")
    },
    new
    {
        id = new Guid("f94e3849-9fe8-47c9-b763-d30b29261af8")
    }
    });

    Console.WriteLine($"{rows} registros deletados");
}

static void CreateManyCategory(SqlConnection connection)
{
    var category = new Category();
    category.Id = Guid.NewGuid();
    category.Title = "Amazon AWS";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços do AWS";
    category.Order = 8;
    category.Summary = "AWS Cloud";
    category.Featured = false;

    var category2 = new Category();
    category2.Id = Guid.NewGuid();
    category2.Title = "Categoria Nova";
    category2.Url = "categoria-nova";
    category2.Description = "Categoria nova";
    category2.Order = 9;
    category2.Summary = "Categoria";
    category2.Featured = true;

    var insertSql = @"INSERT INTO 
            [Category] 
        VALUES(
            @Id,
            @Title,
            @Url,
            @Summary, 
            @Order, 
            @Description, 
            @Featured)";

    var rows = connection.Execute(insertSql, new[]{
    new
    {
        category.Id,
        category.Title,
        category.Url,
        category.Summary,
        category.Order,
        category.Description,
        category.Featured
    },

    new
    {
        category2.Id,
        category2.Title,
        category2.Url,
        category2.Summary,
        category2.Order,
        category2.Description,
        category2.Featured
    }
    });
    Console.WriteLine($"{rows} linhas inseridas");
}

static void ExecuteProcedure(SqlConnection connection)
{
    var procedure = "[spDeleteStudent]";
    var pars = new { StudentId = "3128ffd9-8de1-4468-b5b4-dcf57f67438a" };
    var rows = connection.Execute(procedure, pars, commandType: System.Data.CommandType.StoredProcedure);

    Console.WriteLine($"{rows} linhas foram afetadas");
}

static void ExecuteReadProcedure(SqlConnection connection)
{
    var procedure = "[spGetCoursesByCategory]";
    var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
    var courses = connection.Query(procedure, pars, commandType: CommandType.StoredProcedure);

    foreach (var item in courses)
    {
        Console.WriteLine(item.Id);
    }
}

static void ExecuteScalar(SqlConnection connection)
{
    var category = new Category();
    category.Title = "Amazon AWS";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços do AWS";
    category.Order = 10;
    category.Summary = "AWS Cloud";
    category.Featured = false;

    var insertSql = @"INSERT INTO 
            [Category]
        OUTPUT inserted.[Id] 
        VALUES(
            NEWID(),
            @Title,
            @Url,
            @Summary, 
            @Order, 
            @Description, 
            @Featured)
        SELECT SCOPE_IDENTITY()";

    var id = connection.ExecuteScalar<Guid>(insertSql, new
    {
        category.Title,
        category.Url,
        category.Summary,
        category.Order,
        category.Description,
        category.Featured
    });
    Console.WriteLine($"A categoria adicionada foi: {id}");
}

static void ReadView(SqlConnection connection)
{
    var sql = "SELECT * FROM [vwCourses]";
    var courses = connection.Query(sql);

    foreach (var item in courses)
    {
        Console.WriteLine($"{item.Id} - {item.Title}");
    }
}

static void OneToOne(SqlConnection connection)
{
    var sql = @"SELECT 
                    * 
                FROM 
                    [CareerItem] 
                INNER JOIN 
                    [Course] ON [CareerItem].[CourseId] = [Course].[Id]";

    var items = connection.Query<CareerItem, Course, CareerItem>(
        sql,
        (careerItem, course) =>
        {
            careerItem.Course = course;
            return careerItem;
        }, splitOn: "Id");

    foreach (var item in items)
    {
        Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
    }
}

static void OneToMany(SqlConnection connection)
{
    var sql = @"SELECT 
                    [Career].[Id],
                    [Career].[Title],
                    [CareerItem].[CareerId],
                    [CareerItem].[Title]
                FROM 
                    [Career] 
                INNER JOIN 
                    [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                ORDER BY
                    [Career].[Title]";

    var careers = new List<Career>();
    var items = connection.Query<Career, CareerItem, Career>(
        sql,
        (career, item) =>
        {
            var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
            if (car == null)
            {
                car = career;
                car.Items.Add(item);
                careers.Add(car);
            }
            else
            {
                car.Items.Add(item);
            }
            return career;
        }, splitOn: "CareerId");

    foreach (var career in careers)
    {
        Console.WriteLine($"{career.Title}");
        foreach (var item in career.Items)
        {
            Console.WriteLine($" - {item.Title}");
        }
    }
}

static void QueryMultiple(SqlConnection connection)
{
    var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

    using (var multi = connection.QueryMultiple(query))
    {
        var categories = multi.Read<Category>();
        var courses = multi.Read<Course>();

        foreach (var item in categories)
        {
            Console.WriteLine(item.Title);
        }

        foreach (var item in courses)
        {
            Console.WriteLine(item.Title);
        }
    }
}

static void SelectIn(SqlConnection connection)
{
    var query = "SELECT * FROM Career WHERE [Id] IN @Id";

    var items = connection.Query<Career>(query, new
    {
        Id = new[]{
            "e6730d1c-6870-4df3-ae68-438624e04c72",
            "4327ac7e-963b-4893-9f31-9a3b28a4e72b"
        }
    });

    foreach (var item in items)
    {
        Console.WriteLine(item.Title);
    }
}

static void Like(SqlConnection connection, string term)
{
    var query = "SELECT * FROM Course WHERE [Title] LIKE @Exp";

    var items = connection.Query<Course>(query, new
    {
        Exp = $"%{term}%"
    });

    foreach (var item in items)
    {
        Console.WriteLine(item.Title);
    }
}

static void Transaction(SqlConnection connection)
{
    var category = new Category();
    category.Id = Guid.NewGuid();
    category.Title = "É para não ser inserida";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços do AWS";
    category.Order = 8;
    category.Summary = "AWS Cloud";
    category.Featured = false;

    var insertSql = @"INSERT INTO 
                [Category] 
            VALUES(
                @Id,
                @Title,
                @Url,
                @Summary, 
                @Order, 
                @Description, 
                @Featured)";

    connection.Open();
    using (var transaction = connection.BeginTransaction())
    {
        var rows = connection.Execute(insertSql, new
        {
            category.Id,
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured
        }, transaction);

        // transaction.Commit();
        transaction.Rollback();

        Console.WriteLine($"{rows} linhas inseridas");
    }
}