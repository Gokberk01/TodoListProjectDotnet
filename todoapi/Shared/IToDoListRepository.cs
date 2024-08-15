using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoapi.Models;

namespace todoapi.Shared
{
    public interface IToDoListRepository
    {
        Task<List<ToDo>> GetAllAsync();
        Task<List<ToDo>> GetAllAsyncWithDeletedToDos();
        Task<ToDo> GetByIdAsync(int id);
        Task PostAsync(ToDo newToDo);
        Task UpdateAsync(ToDo updatedToDo);
        Task DeleteAsync(int id);
    }
}