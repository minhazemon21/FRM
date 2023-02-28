--CREATE PROC USP_Test_InsertEmployee
--AS
--BEGIN

--  INSERT INTO EMP_PROFILE
--  (emp_office_code, emp_name, OfficeEmail, emp_datetimeof_birth, emp_joining_datetime, emp_confirmation_datetime, emp_status_id, emp_job_id, emp_desg_id, emp_branch_id, currency_id, added_by, is_salary_stop, IsSalaryDeductForAbsent)
--  VALUES('25456660', 'Minhaz Uddin Emon', 'minhaz@ucasbd.com', '1996-08-21 18:57:03.010', '2022-12-04 18:57:03.010', '2022-10-26 18:57:03.010', 1, 1, 47, 1, 1, 2000, 0, 0 );

--END

ALTER PROC USP_Test_InsertEmployee
AS
BEGIN

  INSERT INTO EMP_PROFILE
  (emp_office_code, emp_name, OfficeEmail, emp_datetimeof_birth, emp_joining_datetime, emp_confirmation_datetime, emp_status_id, emp_job_id, emp_desg_id, emp_branch_id, currency_id, added_by, is_salary_stop, IsSalaryDeductForAbsent, id_card_bar_code)
  VALUES('25456660', 'Minhaz Uddin Emon', 'minhaz@ucasbd.com', '1996-08-21 18:57:03.010', '2022-12-04 18:57:03.010', '2022-10-26 18:57:03.010', 1, 1, 47, 1, 1, 2000, 0, 0, 210);

END

EXEC USP_Test_InsertEmployee

SELECT * FROM EMP_PROFILE




EXEC USP_INSERT_Test_Employee '250', 'Minhaz Uddin Emon', 'minhaz@ucasbd.com', '1996-08-21 18:57:03.010', '2022-12-04 18:57:03.010', '2022-10-26 18:57:03.010', 1, 1, 47, 1, 1, 2000, 0, 0, 212

CREATE PROC USP_INSERT_Test_Employee
@emp_office_code nvarchar(15),
@emp_name nvarchar(100),
@OfficeEmail nvarchar(200),
@emp_datetimeof_birth datetime,
@emp_joining_datetime datetime,
@emp_confirmation_datetime datetime,
@emp_status_id numeric(10, 0),
@emp_job_id int,
@emp_desg_id int,
@emp_branch_id int,
@currency_id int,
@added_by int,
@is_salary_stop numeric(1, 0),
@IsSalaryDeductForAbsent bit,
@id_card_bar_code nvarchar(30)

AS
BEGIN

INSERT INTO EMP_PROFILE
  (emp_office_code, emp_name, OfficeEmail, emp_datetimeof_birth, emp_joining_datetime, emp_confirmation_datetime, emp_status_id, emp_job_id, emp_desg_id, emp_branch_id, currency_id, added_by, is_salary_stop, IsSalaryDeductForAbsent, id_card_bar_code)
  VALUES(@emp_office_code, @emp_name, @OfficeEmail, @emp_datetimeof_birth, @emp_joining_datetime, @emp_confirmation_datetime, @emp_status_id, @emp_job_id, @emp_desg_id, @emp_branch_id, @currency_id, @added_by, @is_salary_stop, @IsSalaryDeductForAbsent, @id_card_bar_code)

END

SELECT * from EMP_PROFILE
WHERE emp_name LIKE '__NH%';

SELECT * FROM EMP_PROFILE
WHERE emp_dept_id IN (1,9,5);
