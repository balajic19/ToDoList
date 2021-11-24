using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TodoController : Controller
    {

        private readonly TodoContext context;

        public TodoController(TodoContext context)
        {
            this.context = context;
        }

       // GET
        public async Task<ActionResult> Index()
        {
            IQueryable<TodoList> items = from i in context.TodoList orderby i.Id select i;

            List<TodoList> todoList = await items.ToListAsync();

            return View(todoList);
        }

        //GET /TODO /CREATE
        public IActionResult Create() => View();

        //POST /todo/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "This item has been added";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET /todo/edit/5
        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await context.TodoList.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);

        }

        //POST /todo/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "This item has been updated";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET /todo/delete/5
        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await context.TodoList.FindAsync(id);

            if (item == null)
            {
                TempData["Error"] = "This item does not exist";
            }
            else
            {
                context.TodoList.Remove(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "This item has been Deleted";
            }

            return RedirectToAction("Index");

        }
    }
}
