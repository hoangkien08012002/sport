﻿using HightSportShopBusiness.Interfaces;
using HightSportShopBusiness.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HightSportShopBusiness.Services
{
    public interface IImageService
    {
        bool UploadImages(List<IFormFile> images, string folderPath, ImagesVideo image);
    }

    public class ImageService : IImageService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly HighSportShopDBContext _dBContext;

        public ImageService(IUnitOfWork unitOfWork, HighSportShopDBContext dBContext)
        {
            _unitOfWork = unitOfWork;
            _dBContext = dBContext;
        }

        public bool UploadImages(List<IFormFile> images, string folderPath, ImagesVideo image)
        {
            if (images == null || images.Count == 0)
            {
                return false;
            }

            foreach (var file in images)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(folderPath, "images", file.FileName);

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }

            return true;
        }
    }
}
