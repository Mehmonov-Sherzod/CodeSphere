using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Video;
using CodeSphere.DataAccess.Persistence;
using CodeSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service.Impl
{
    public class VideoService : IVideoService
    {
        private readonly IFileStoreageService _fileStoreageService;
        private readonly AppDbContext _appDbContext;

        public VideoService(IFileStoreageService fileStoreageService, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _fileStoreageService = fileStoreageService;
        }

        public async Task<ApiResult<string>> CreateVideo(CreateVideo createVideo)
        {
            var topic = await _appDbContext.Topic.FirstOrDefaultAsync(x => x.Id == createVideo.TopicId);

            if (topic == null)
                return ApiResult<string>.Failure(new List<string> { "Topic not found" });

            string? urlImage = null;

            if (createVideo.Image != null && createVideo.Image.Length > 0)
            {
                var extension = Path.GetExtension(createVideo.Image.FileName);
                var objectName = $"{Guid.NewGuid()}{extension}";
                using var mystream = createVideo.Image.OpenReadStream();
                urlImage = await _fileStoreageService.UploadFileAsync(
                    "questions-image",
                    objectName,
                    mystream,
                    createVideo.Image.ContentType
                );
            }

            string? videoUrl = null;

            if (createVideo.Video != null && createVideo.Video.Length > 0)
            {
                var videoExtension = Path.GetExtension(createVideo.Video.FileName);
                var videoObjectName = $"{Guid.NewGuid()}{videoExtension}";
                using var videoStream = createVideo.Video.OpenReadStream();
                videoUrl = await _fileStoreageService.UploadFileAsync(
                    "videos",
                    videoObjectName,
                    videoStream,
                    createVideo.Video.ContentType
                );
            }

            var video = new Videos
            {
                Name = createVideo.Name,
                ImageUrl = urlImage,
                VideoUrl = videoUrl,
                TopicId = createVideo.TopicId
            };

            await _appDbContext.Videos.AddAsync(video);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Video has been created");
        }

        public async Task<ApiResult<PaginationResult<GetVideo>>> GetAllVideoPage(PageOption pageOption)
        {
            var query = _appDbContext.Videos.AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(s => s.Name.Contains(pageOption.Search));
            }

            List<GetVideo> videos = await query
               .Skip(pageOption.PageSize * (pageOption.PageNumber - 1))
               .Take(pageOption.PageSize)
               .Select(x => new GetVideo
               {
                   Id = x.Id,
                   Name = x.Name,
                   ImageUrl = x.ImageUrl,
                   VideoUrl = x.VideoUrl,
                   TopicId = x.TopicId
               }).ToListAsync();

            int total = await _appDbContext.Videos.CountAsync();

            var pagination = new PaginationResult<GetVideo>
            {
                PageSize = pageOption.PageSize,
                PageNumber = pageOption.PageNumber,
                TotalCount = total,
                Values = videos
            };

            return ApiResult<PaginationResult<GetVideo>>.Success(pagination);
        }

        public async Task<ApiResult<string>> UpdateVideo(UpdateVideo updateVideo, Guid id)
        {
            var video = await _appDbContext.Videos.FirstOrDefaultAsync(x => x.Id == id);

            if (video == null)
                return ApiResult<string>.Failure(new List<string> { "Video not found" });

            var topic = await _appDbContext.Topic.FirstOrDefaultAsync(x => x.Id == updateVideo.TopicId);

            if (topic == null)
                return ApiResult<string>.Failure(new List<string> { "Topic not found" });

            string? urlImage = null;

            if (updateVideo.Image != null && updateVideo.Image.Length > 0)
            {
                var extension = Path.GetExtension(updateVideo.Image.FileName);
                var objectName = $"{Guid.NewGuid()}{extension}";
                using var mystream = updateVideo.Image.OpenReadStream();
                urlImage = await _fileStoreageService.UploadFileAsync(
                    "questions-image",
                    objectName,
                    mystream,
                    updateVideo.Image.ContentType
                );
            }

            string? videoUrl = null;

            if (updateVideo.Video != null && updateVideo.Video.Length > 0)
            {
                var videoExtension = Path.GetExtension(updateVideo.Video.FileName);
                var videoObjectName = $"{Guid.NewGuid()}{videoExtension}";
                using var videoStream = updateVideo.Video.OpenReadStream();
                videoUrl = await _fileStoreageService.UploadFileAsync(
                    "videos",
                    videoObjectName,
                    videoStream,
                    updateVideo.Video.ContentType
                );
            }

            video.Name = updateVideo.Name;
            video.ImageUrl = urlImage ?? video.ImageUrl;
            video.VideoUrl = videoUrl ?? video.VideoUrl;
            video.TopicId = updateVideo.TopicId;

            _appDbContext.Update(video);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Video has been updated");
        }

        public async Task<ApiResult<string>> DeleteVideo(Guid id)
        {
            var video = await _appDbContext.Videos.FirstOrDefaultAsync(x => x.Id == id);

            if (video == null)
                return ApiResult<string>.Failure(new List<string> { "Video not found" });

            _appDbContext.Videos.Remove(video);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Video has been deleted");
        }
    }
}
