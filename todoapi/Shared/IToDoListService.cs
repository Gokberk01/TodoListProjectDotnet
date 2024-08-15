using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todoapi.Models;

namespace todoapi.Shared
{
    public interface IToDoListService
    {
        Task<List<ToDo>> GetToDosAsync();
        Task<List<ToDo>> GetAllAsyncWithDeletedToDos();
        Task<ToDo> GetToDoByIdAsync(int id);
        Task<ToDo> PostToDoAsync(ToDo newToDo);
        Task<ToDo> UpdateToDoAsync(int id, ToDo updatedToDo);
        Task<ToDo> DeleteToDoAsync(int id);
    }
}