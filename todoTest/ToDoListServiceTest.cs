using System.Collections;
using System.IO.Compression;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using todoapi.Data;
using todoapi.Models;
using todoapi.Repositories;
using todoapi.Services;
using todoapi.Shared;
using Xunit;
using Xunit.Sdk;


namespace todoTest;

public class ToDoListServiceTest
{

    private readonly Mock<IToDoListRepository> _repository;
    private readonly ToDoListService _service;

    public ToDoListServiceTest()
    {

        _repository = new Mock<IToDoListRepository>();

        _service = new ToDoListService(_repository.Object);
    }

    [Fact]
    public async Task GetToDosAsync_ReturnsAllToDos()
    {
        // Arrange
        List<ToDo> toDoList = new List<ToDo> 
        {
            new ToDo { ToDoID = 1, ToDoContent = "New Task", IstoDoDone = false }, 
            new ToDo { ToDoID = 2, ToDoContent = "Second New Task", IstoDoDone = true }
        };


        
        _repository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(toDoList);

        //Act
        var result = await _service.GetToDosAsync();

        //Asset
        Assert.Equal(result.Count, toDoList.Count);
     

    }
    

    [Fact]
    public async Task GetToDoByIdAsync_ReturnsToDo()
    {
        // Arrange
        List<ToDo> toDoList = new List<ToDo> 
        {
            new ToDo { ToDoID = 1, ToDoContent = "New Task", IstoDoDone = false }, 
            new ToDo { ToDoID = 2, ToDoContent = "Second New Task", IstoDoDone = true }
        };

        _repository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(toDoList[0]);

        // Act
        var result = await _service.GetToDoByIdAsync(1);

        // Assert
        Assert.Equal(toDoList[0], result);
        _repository.Verify(s => s.GetByIdAsync(1), Times.Once);


    }

    [Fact]
    public async Task GetToDoById_Null_Exception()
    {
        // Arrange 
        List<ToDo> toDoList = new List<ToDo> 
        {
            new ToDo { ToDoID = 1, ToDoContent = "New Task", IstoDoDone = false }, 
            new ToDo { ToDoID = 2, ToDoContent = "Second New Task", IstoDoDone = true }
        };

        // Act  
        //----Func<Task> türü parametre almayan ve Task döndüren bir methodu temsil eder.
        //Temsilci: Lambda( => ()) ifadesi kullanarak Func<Task> türünde bir temsilci oluşturuyorsunuz. Bu, act değişkenine atanan bir metot gibi davranır, 
        //ancak bu metdonun çağrılması act() şeklinde olur.
        Func<Task> act =  () => _service.GetToDoByIdAsync(5);

        // Assert
        //Assert.ThrowsAsync<Exception> metoduna Func<Task> türünde bir temsilci (delegate) sağlamanız gerekir. 
        //Bu temsilci, belirli bir kod bloğunu temsil eder ve bu kod bloğu çalıştırıldığında bir Task döndürmelidir.
        //deferredTask: () => _service.GetToDoByIdAsync(5) ifadesi bir lambda ifadesi olarak tanımlanır ve 
        //deferredTask değişkenine atanır. Bu ifade henüz çalıştırılmadı, sadece temsilci oluşturuldu.
        //deferredTask() ifadesi çağrıldığında, GetToDoByIdAsync(5) metodu çalıştırılır ve Task döner.
        var exception = await Assert.ThrowsAsync<Exception> (act);

    }

    [Fact]
    public async Task PostToDoAsync_AddsToDo()
    {
        // Arrange
        var newToDo = new ToDo { ToDoID = 1, ToDoContent = "New Task", IstoDoDone = false };

        _repository.Setup(m => m.PostAsync(newToDo)).Verifiable();

        // Act
        var result = await _service.PostToDoAsync(newToDo);

        // Assert
        Assert.Equal(newToDo, result);
    }

    [Fact]
    public async Task UpdateToDoAsync_UpdatesExistingToDo()
    {
        // Arrange
        var oldToDo = new ToDo { ToDoID = 5, ToDoContent = "Old Task", IstoDoDone = false };
        var updatedToDo = new ToDo { ToDoID = 5, ToDoContent = "New Task", IstoDoDone = true };

        _repository.Setup(s => s.GetByIdAsync(5)).ReturnsAsync(oldToDo);
        _repository.Setup(s => s.UpdateAsync(oldToDo));



        // Act
        var resultFromUpdate = await _service.UpdateToDoAsync(5,updatedToDo);

        // Assert
        Assert.Equal(updatedToDo, resultFromUpdate); 
    }

    [Fact]
    public async Task UpdateToDoAsync_Unmatch_Id()
    {
        // Arrange
        var oldToDo = new ToDo { ToDoID = 5, ToDoContent = "Old Task", IstoDoDone = false };
        var updatedToDo = new ToDo { ToDoID = 6, ToDoContent = "New Task", IstoDoDone = true };

    
        // Act
        Func<Task> act = () => _service.UpdateToDoAsync(5, updatedToDo);
    
        // Assert
        var exception = await Assert.ThrowsAsync<Exception> (act);
    }

    [Fact]
    public async Task UpdateToDoAsync_Null_Exception()
    {
        // Arrange
        var oldToDo = new ToDo { ToDoID = 5, ToDoContent = "Old Task", IstoDoDone = false };
        var updatedToDo = new ToDo { ToDoID = 5, ToDoContent = "New Task", IstoDoDone = true };

    
        // Act
        Func<Task> act = () => _service.UpdateToDoAsync(2, updatedToDo);
    
        // Assert
        var exception = await Assert.ThrowsAsync<Exception> (act);
    }


    [Fact]
    public async Task DeleteToDoAsync_DeleteToDo()
    {
        // Arrange
        var deletedToDo = new ToDo { ToDoID = 5, ToDoContent = "New Task", IstoDoDone = false };


        _repository.Setup(s => s.GetByIdAsync(5)).ReturnsAsync(deletedToDo);
        _repository.Setup(s => s.DeleteAsync(5));

        // Act
        var resultFromDelete = await _service.DeleteToDoAsync(5);

        // Assert
        Assert.Equal(deletedToDo, resultFromDelete);
    }

    [Fact]
    public async Task DeleteToDoAsync_Null_Exception()
    {
        // Arrange
        var deletedToDo = new ToDo { ToDoID = 5, ToDoContent = "New Task", IstoDoDone = false };

        // Act
        Func<Task> act = () => _service.DeleteToDoAsync(2);

        // Assert
        var exception = await Assert.ThrowsAsync<Exception> (act);
    }


}
