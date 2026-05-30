using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Entites.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.ViewModel.Area.Model.Dto;
using Services.ViewModel.Area.Model.Request;
using System.Linq;
using WebFramework.Filter;
using X.PagedList.Extensions;

namespace StoreTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiResultFilter]
    [Authorize(Roles = "Admin,Opratoe")]

    public class CategoryController : Controller
    {
        private readonly ICategoryRepository category;
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository category, IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.category = category;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index(int? ParentId,int? page)
        {
            var categores = category.TableNoTracking
                .Include(x=>x.SubCategory)
                .Include(x=>x.ParentCategory)
                .Where(x=>x.ParentCategoryId==ParentId)
                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                .ToPagedList(page??1,2);

            var result = new RequestCategoryDto
            {
                CategoryDtos = categores
            };

            return View(result);

        }
   
        [HttpPost]
        public async Task<IActionResult> Create(int? ParentId, string Name, CancellationToken cancellationToken)
        {
            //var parent =await category.GetByIdAsync(cancellationToken,ParentId);

            //var categorys = new Category
            //{
            //    Id = parent.Id,
            //    Name = $"{parent.ParentCategory?.Name ?? "بدون والد"} - {parent.Name}"

            //};

            if (ParentId != null)
            {
                var parent = await category.GetByIdAsync(cancellationToken, ParentId);
                var categorys = new Category
                {
                   Name=Name,
                   ParentCategoryId=parent.Id
                    

                };
                await category.AddAsync(categorys, cancellationToken);
                return Ok();

            }
            var newcategory = new Category 
            {
                Name = Name,

            };
            await category.AddAsync(newcategory, cancellationToken);

            return Ok();



        }
        [HttpGet]
        public IActionResult Create(int? ParentId)
        {
            ViewBag.ParentId = ParentId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id,CancellationToken cancellationToken) 
        {
            var categories =await category.Table.Include(x=>x.SubCategory).FirstOrDefaultAsync(x=>x.Id==Id);
            if (categories==null)
            {
                return NotFound();
            }
            categories.IsDelete = true;

            if (categories.SubCategory!=null)
            {

                foreach (var item in categories.SubCategory)
                {
                    item.IsDelete = true;
                }
            }

            await categoryRepository.UpdateAsync(categories,cancellationToken);
            

            return Ok();
        }
        [HttpPost]

        public async Task<IActionResult> Edit(int Id , string Name,CancellationToken cancellationToken)
        { 
            var categories=category.GetById(Id);
            categories.Name=Name;

            await category.UpdateAsync(categories,cancellationToken);

            return Ok();


        }
    }
    }
