﻿using AutoMapper;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Ambar;
using Warehouse.Data.Dto.Category;
using Warehouse.Data.Dto.Products;
using Warehouse.Data.Dto.Transactions;
using Warehouse.Data.Models;

namespace Warehouse.Data.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Ambar
            CreateMap<Anbar,AnbarCreateDto>();
            CreateMap<Anbar, AnbarUpdateDto>();

            CreateMap<AnbarCreateDto, Anbar>();
            CreateMap<AnbarUpdateDto, Anbar>();

            //Category
            CreateMap<Category, CategoryCreateDto>();
            CreateMap<Category, CategoryUpdateDto>();

            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

            //Product
            CreateMap<Product, ProductCreateDto>();
            CreateMap<Product, ProductUpdateDto>();

            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();



            //Transaction
            CreateMap<Transaction, TransactionCreateDto>();
            CreateMap<Transaction, TransactionUpdateDto>();

            CreateMap<TransactionCreateDto, Transaction>();
            CreateMap<TransactionUpdateDto, Transaction>();
        }

    }
}
