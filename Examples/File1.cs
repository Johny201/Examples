/// <summary>
/// Remove all data related to cell with id = cell_id
/// </summary>
/// <param name="cell_id"></param>
/// <returns>Return true if method did work without exceptions</returns>
public bool RemoveRelatedToCellData(int cell_id)
{
    try
    {
        using (DB.ShelfContext context = new DB.ShelfContext(connect, false))
        {
            var cell = context.CELLS.FirstOrDefault(c => c.cell_id == cell_id);
            if (cell != null)
            {
                cell.status_type_id = (int)Constants.CELL_STATUS.EMPTY;
            }

            var orders = context.ORDERS.Where(o => o.cell_id == cell_id).ToList();
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    var belongings = context.BELONGINGS.Where(b => b.order_id == order.order_id).ToList();
                    if (belongings.Count > 0)
                    {
                        foreach (var belonging in belongings)
                        {
                            context.BELONGINGS.Remove(belonging);
                        }
                    }

                    var shelf_books = context.SHELF_BOOKS.Where(sb => sb.order_id == order.order_id).ToList();
                    if (shelf_books != null)
                    {
                        foreach (var shelf_book in shelf_books)
                        {
                            context.SHELF_BOOKS.Remove(shelf_book);
                        }
                    }
                    context.ORDERS.Remove(order);
                }
            }

            var lock_entries = context.LOCK_ENTRIES.Where(le => le.cell_id == cell_id).ToList();
            if (lock_entries != null)
            {
                foreach (var lock_entry in lock_entries)
                {
                    var shelf_books = context.SHELF_BOOKS.Where(sb => sb.order_id == lock_entry.lock_entry_id).ToList();
                    if (shelf_books != null)
                    {
                        foreach (var shelf_book in shelf_books)
                        {
                            context.SHELF_BOOKS.Remove(shelf_book);
                        }
                    }

                    context.LOCK_ENTRIES.Remove(lock_entry);
                }
            }

            context.SaveChanges();

            return true;
        }
    }
    catch (Exception ex)
    {

    }

    return false;
}

/// <summary>
/// Get data on locker department if any set
/// </summary>
/// <param name="locker_id"></param>
/// <returns>Data about locker department. Note that existing data has id > 0</returns>
public DepartmentData GetLockerDepartment(int locker_id)
{
    try
    {
        using (DB.ShelfContext context = new DB.ShelfContext(connect, false))
        {
            var department_data = new DepartmentData();
            var department = context.LOCKERS.Include("department")
                .FirstOrDefault(l => l.locker_id == locker_id)?.department;

            if (department != null)
            {
                department_data.department_id = department.department_id;
                department_data.department_name = department.department_name;
                department_data.address = department.address;
                department_data.email_address = department.email_address;
                department_data.email_host = department.email_host;
                department_data.email_port = department.email_port;
                department_data.email_user = department.email_user;
                department_data.email_password = department.email_password;
            }

            return department_data;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("GetLockerDepartment failed: " + ex.Message);
        return new DepartmentData();
    }
}
