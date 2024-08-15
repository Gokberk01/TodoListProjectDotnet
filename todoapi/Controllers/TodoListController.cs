using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoapi.Models;
using todoapi.Services;
using todoapi.Shared;
using todoapi.Shared.Dtos;

namespace todoapi.Controllers
{
    [Route("api/[controller]")]  //Burda "api/[controller]" derken controllerdan kastı controller sınıfının
    [ApiController] //controller yazısı atılmış hali yani sınıfım todoListController o yüzden
    [Authorize]           
    public class TodoListController : ControllerBase //todolist oluyor hepsi lowercase oluyor -> api/todolist
    {
        private readonly IToDoListService toDoListService; //I ekledim 18 ve 22.satıra !!
        private readonly JwtService _jwtService;
        
        public TodoListController(IToDoListService toDoListService, JwtService jwtService)
        {
            this.toDoListService = toDoListService;
            _jwtService = jwtService;
        }

        //Get: api/todoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDoList ()
        {
           var ToDoList = await toDoListService.GetToDosAsync();
           return Ok(ToDoList);
        }

        //Get api/todolist/1
        [HttpGet("{id}")]

        public async Task<ActionResult<ToDo>> GetToDobyId(int id)
        {
            
            try
            {
                var _ToDo = await toDoListService.GetToDoByIdAsync(id);
                return Ok(_ToDo);
            }
            catch (Exception ex)
            {
                
                return NotFound(new {message = ex.Message});
            }
        }


        // POST: api/todolist
        [HttpPost]
        public async Task<ActionResult<ToDo>> PostTodoItem(TodoDto todoDto)
        {
            var todoItem = new ToDo 
            {
                ToDoContent = todoDto.context,
                IstoDoDone = todoDto.IstoDoDone,
                IsDeleted = todoDto.IsDeleted
            };
            
           var addedToDo = await toDoListService.PostToDoAsync(todoItem);
           return Created("success", addedToDo);

        }

        //Update api/todolist/3
        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> UpdateToDoList(ToDo Updatedtodo)
        {
            try
            {
                var updatedTodo = await toDoListService.UpdateToDoAsync(Updatedtodo.ToDoID , Updatedtodo);
                return Ok(updatedTodo);
            }
            catch (Exception ex)
            {
                
                return NotFound(new {message = ex.Message});
            }

        }


        //Delete api/todolist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {

            try
            {
                var deletedTodo = await toDoListService.DeleteToDoAsync(id);
                return Ok(deletedTodo);
            }
            catch (Exception ex)
            {
                
                return NotFound(new {message = ex.Message});
            }
        }
    }
}