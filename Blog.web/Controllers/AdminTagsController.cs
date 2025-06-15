using System.ComponentModel;
using Blog.web.Data;
using Blog.web.Models.Domain;
using Blog.web.Models.ViewModel;
using Blog.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }


        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            //this is done to Map the data that is filled in the View Model(AddTagRequest) to the actual model(Tag) in Domain folder...
            //Here we create the variable for the actual model class Tag of the Domain folder
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }


        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            //We use dbCOntext to read the Tags form the dataBase
            //Here we use the previously created BlogDbCOntext class object blogDbContext to reterive the data from the database
            //And store it to the variable called tags

            var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //this search the data in the tag table in database with matching id
            var tag = await tagRepository.GetAsync(id);
            //And this checks that tag is null or not 
            //If not null Save the data of the actual model(tag) and store it to the View model(EditTagRequest) properties
            if (tag != null)
            {
                //Create a new object variable of the model class (EditTagRequest)
                //And save the data of the 
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName

                };
                return View(editTagRequest);

            }
            return View(null);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            //this is done beacuse value at viewmodel(EditTagRequest) we have to save it to the acutal model(Tag)
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,

            };

            //this checks either there is any tags on the database or not
            //If there exists then store to the existingTag variable
            var updatedTag= await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
            }
            else
            {

            }


            return RedirectToAction("List", new { id = editTagRequest.Id });
            


        }

        [HttpPost]
        public async Task<IActionResult> Delete (EditTagRequest editTagRequest)
        {
          var deletedTag= await tagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                return RedirectToAction("List");
            }
            

            
                return RedirectToAction("Edit",new {id=editTagRequest.Id});
        }
    }   
}
