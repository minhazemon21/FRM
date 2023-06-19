from tkinter import *
from tkinter import messagebox

def register_student():
    first_name = first_name_entry.get()
    last_name = last_name_entry.get()
    title = title_var.get()
    age = age_entry.get()
    nationality = nationality_var.get()
    registration_status = registration_status_var.get()
    completed_courses = completed_courses_entry.get()
    semester = semester_entry.get()
    terms_checked = terms_var.get()

    # Validate inputs
    if not first_name or not last_name or not age or not completed_courses or not semester:
        messagebox.showerror("Error", "Please fill in all fields.")
        return

    # Write student information to file
    with open("students.txt", "a") as file:
        file.write(f"First Name: {first_name}\n")
        file.write(f"Last Name: {last_name}\n")
        file.write(f"Title: {title}\n")
        file.write(f"Age: {age}\n")
        file.write(f"Nationality: {nationality}\n")
        file.write(f"Registration Status: {'Yes' if registration_status else 'No'}\n")
        file.write(f"Completed Courses: {completed_courses}\n")
        file.write(f"Semester: {semester}\n")
        file.write(f"Terms and Conditions: {'Accepted' if terms_checked else 'Not Accepted'}\n\n")

    # Clear input fields
    first_name_entry.delete(0, END)
    last_name_entry.delete(0, END)
    age_entry.delete(0, END)
    completed_courses_entry.delete(0, END)
    semester_entry.delete(0, END)
    terms_checkbutton.deselect()

    messagebox.showinfo("Success", "Student registered successfully.")

# Create the main window
window = Tk()
window.title("Student Registration System")

# Create labels
first_name_label = Label(window, text="First Name:")
first_name_label.grid(row=0, column=1, sticky=E, padx=10, pady=10)

last_name_label = Label(window, text="Last Name:")
last_name_label.grid(row=0, column=3, sticky=E, padx=10, pady=10)

title_label = Label(window, text="Title:")
title_label.grid(row=0, column=5, sticky=E, padx=10, pady=10)

age_label = Label(window, text="Age:")
age_label.grid(row=2, column=1, sticky=E, padx=10, pady=10)

nationality_label = Label(window, text="Nationality:")
nationality_label.grid(row=2, column=3, sticky=E, padx=10, pady=10)

registration_status_label = Label(window, text="Registration Status:")
registration_status_label.grid(row=2, column=5, sticky=E, padx=10, pady=10)

completed_courses_label = Label(window, text="Completed Courses:")
completed_courses_label.grid(row=4, column=1, sticky=E, padx=10, pady=10)

semester_label = Label(window, text="Semester:")
semester_label.grid(row=4, column=3, sticky=E, padx=10, pady=10)

terms_label = Label(window, text="Terms and Conditions:")
terms_label.grid(row=6, column=0, sticky=E, padx=10, pady=10)

# Create entry fields
first_name_entry = Entry(window)
first_name_entry.grid(row=1, column=1, padx=10, pady=10)

last_name_entry = Entry(window)
last_name_entry.grid(row=1, column=3, padx=10, pady=10)

title_var = StringVar()
title_dropdown = OptionMenu(window, title_var, "Mr", "Mrs")
title_dropdown.grid(row=1, column=5, sticky=W, padx=10, pady=10)

age_entry = Entry(window)
age_entry.grid(row=3, column=1, padx=10, pady=10)

nationality_var = StringVar()
nationality_dropdown = OptionMenu(window, nationality_var, "Bangladesh", "USA", "Germany")
nationality_dropdown.grid(row = 3, column=3, padx=10, pady=10)

# Create registration status checkbox
registration_status_var = BooleanVar()
registration_status_checkbox = Checkbutton(window, variable=registration_status_var, text="Registered")
registration_status_checkbox.grid(row=3, column=5, sticky=W, padx=10, pady=10)

completed_courses_entry = Entry(window)
completed_courses_entry.grid(row=5, column=1, padx=10, pady=10)

semester_entry = Entry(window)
semester_entry.grid(row=5, column=3, padx=10, pady=10)

terms_var = BooleanVar()
terms_checkbutton = Checkbutton(window, variable=terms_var, text="I accept the Terms and Conditions")
terms_checkbutton.grid(row=6, column=1, sticky=W, padx=10, pady=10)

# Create register button
register_button = Button(window, text="Register", command=register_student)
register_button.grid(row=9, columnspan=2, padx=10, pady=10)

# Start the main loop
window.mainloop()
