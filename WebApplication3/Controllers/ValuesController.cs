﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> UploadFileToS3(IFormFile file)
        {
            
            // access key id and secret key id, can be generated by navigating to IAM roles in AWS and then add new user, select permissions
            //for this example, try giving S3 full permissions
            using (var client = new AmazonS3Client("AKIATLRNDGXDBfsdfdsfsdWKJX6YO", "X+su4p9f5sdfsdf4D1/s4q5Z/ZhmZLGWgamR4Wrg7/xONt", RegionEndpoint.EUWest1))
            {

                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = file.FileName, // filename
                        //Key = "image/"+file.FileName, // folder 

                        BucketName = "firstbuckets1", // bucket name of S3
                                    
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                    
                }
            }
            
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteFileToS3([FromForm] string file)
        {
            using (var client = new AmazonS3Client("AKIATLRNDGXDBWsdfsdKJX6YO", "X+su4psdfsdf9f54D1/s4q5Z/ZhmZLGWgamR4Wrg7/xONt", RegionEndpoint.EUWest1))
            {
               await client.DeleteObjectAsync("firstbuckets1",file);
               //await client.DeleteObjectAsync("firstbuckets1","image/"+file);// folder delete
               
            }
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetFilToS3([FromForm] string file)
        {
            using (var client = new AmazonS3Client("AKIATLRNDGXDBWsdfsKJX6YO", "X+su4p9f54D1sdf/s4q5Z/ZhmZLGWgamR4Wrg7/xONt", RegionEndpoint.EUWest1))
            {
               


                var request = new GetPreSignedUrlRequest
                {
                    BucketName = "firstbuckets1",
                    Key = file,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.UtcNow.AddDays(7)

                };
                string url = client.GetPreSignedURL(request);
                return Ok(url);

            }
           


        }
        
    }
}