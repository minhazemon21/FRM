from tkinter import *
from tkinter import messagebox

def register_student():
    name = name_entry.get()
    student_id = id_entry.get()
    course = course_entry.get()

    # Validate inputs
    if not name or not student_id or not course:
        messagebox.showerror("Error", "Please fill in all fields.")
        return

    # Write student information to file
    with open("students.txt", "a") as file:
        file.write(f"Name: {name}\nID: {student_id}\nCourse: {course}\n\n")

    # Clear input fields
    name_entry.delete(0, END)
    id_entry.delete(0, END)
    course_entry.delete(0, END)

    messagebox.showinfo("Success", "Student registered successfully.")

# Create the main window
window = Tk()
window.title("Student Registration System")

# Create labels
name_label = Label(window, text="Name:")
name_label.grid(row=0, column=0, sticky=E)

id_label = Label(window, text="ID:")
id_label.grid(row=1, column=0, sticky=E)

course_label = Label(window, text="Course:")
course_label.grid(row=2, column=0, sticky=E)

# Create entry fields
name_entry = Entry(window)
name_entry.grid(row=0, column=1)

id_entry = Entry(window)
id_entry.grid(row=1, column=1)

course_entry = Entry(window)
course_entry.grid(row=2, column=1)

# Create register button
register_button = Button(window, text="Register", command=register_student)
register_button.grid(row=3, columnspan=2)

# Start the main loop
window.mainloop()
