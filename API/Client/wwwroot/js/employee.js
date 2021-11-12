$(document).ready(function () {
    $('#dataTable1').DataTable({
        'ajax': {
            'url': 'Employees/GetAll',
            'dataSrc': ''
        },
        'dom': '<lf<t>ip>',
        'buttons': [
            {
                extend: 'excelHtml5',
                name: 'excel',
                title: 'Employee',
                sheetName: 'Employee',
                text: '',
                className: 'buttonHide btn-default ',
                filename: 'Data',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            }

        ],
        'columnDefs': [{
            'targets': [0, 8],
            'orderable': false
        }],
        'columns': [
            {
                'data': null,
                'render': function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                'data': "nik"
            },
            {
                'data': "",
                'render': function (data, type, row, meta) {
                    return row['firstName'] + ' ' + row['lastName'];
                }
            },
            {
                'data': '',
                'render': function (data, type, row, meta) {
                    if (row['phone'].substr(0, 1) == '0') {
                        return '+62' + row['phone'].substr(1);
                    }
                    else {
                        return '+62' + row['phone'];
                    }
                }
            },
            {
                'data': '',
                'render': function (data, type, row, meta) {
                    var date = row['birthDate'].substr(0, 10);
                    var newDate = date.split('-');
                    return newDate[2] + '-' + newDate[1] + '-' + newDate[0];
                }
            },
            {
                'data': '',
                'render': function (data, type, row, meta) {
                    return 'Rp. ' + row['salary'].toLocaleString();
                }
            },
            {
                'data': 'email'
            },
            {
                'data': '',
                'render': function (data, type, row, meta) {
                    if (row['gender'] == 0) {
                        return 'Male';
                    } else {
                        return 'Female';
                    }
                }
            },
            {
                'data': null,
                'render': function (data, type, row) {
                    return `<button type="button" class="btn btn-info" data-toggle="tooltip" data-placement="left" title="Update" onclick="getEmployee('${row['nik']}')"><i class="fas fa-edit"></i></button>
                            <button type="button" class="btn btn-danger" data-toggle="tooltip" data-placement="left" title="Delete" onclick="deleteEmployee('${row['nik']}')"><i class="fas fa-trash-alt"></i></button>`;
                }
            }
        ]
    });
    $.validator.addMethod("check_email", function (value, element) {
        let email = value;
        if (!(/^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(email))) {
            return false;
        }
        return true;
    }, function (value, element) {
        let email = $(element).val();
        if (!(/^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(email))) {
            return "Please enter your valid email."
        }
        return false;
    });

    $.validator.addMethod("strong_password", function (value, element) {
        let password = value;
        if (!(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%&*!])(.{8,20}$)/.test(password))) {
            return false;
        }
        return true;
    }, function (value, element) {
        let password = $(element).val();
        if (!(/^(.{8,20}$)/.test(password))) {
            return 'Password must be between 8 and 20 characters long.';
        }
        else if (!(/^(?=.*[A-Z])/.test(password))) {
            return 'Password must contain atleast one uppercase.';
        }
        else if (!(/^(?=.*[a-z])/.test(password))) {
            return 'Password must contain atleast one lowercase.';
        }
        else if (!(/^(?=.*[0-9])/.test(password))) {
            return 'Password must contain atleast one digit.';
        }
        else if (!(/^(?=.*[@#$%&*!])/.test(password))) {
            return "Password must contain special characters from @#$%&*!.";
        }
        return false;
    });
});

function Validate() {
    var ini = $("#register").valid();
    console.log(ini);

    if (ini === true) {
        insertData();
    }
    else {
        Swal.fire(
            'Failed!',
            'Please enter all fields.',
            'error'
        );
    }
};

function ValidateUpdate(id) {
    var ini = $("#update").valid();
    console.log(ini);

    if (ini === true) {
        update(id);
    }
    else {
        Swal.fire(
            'Failed!',
            'Please enter all fields.',
            'error'
        );
    }
};


$.ajax({
    url: "Universities/GetAll",
    success: function (result) {
        console.log(result);
        var namaUniv = "";
        $.each(result, function (key, val) {
            namaUniv += `<option value="${val.id}">${val.name}</option>`
        });
        $("#inputUnivId").html(namaUniv);
    }
})

$.ajax({
    url: "Roles/GetAll",
    success: function (result) {
        console.log(result);
        var namaRole = "";
        $.each(result, function (key, val) {
            namaRole += `<option value="${val.id}">${val.roleName}</option>`
        });
        $("#inputRoleId").html(namaRole);
    }
})

function clearTextBox() {
    $('#inputNIK').val("");
    $('#inputFirstName').val("");
    $('#inputLastName').val("");
    $('#inputPhone').val("");
    $('#inputBirthDate').val("");
    $('#inputSalary').val("");
    $('#inputEmail').val("");
    $('#inputPassword').val("");
    $('#inputDegree').val("");
    $('#inputGPA').val("");/*
    $('#inputUnivId').val("");
    $('#inputRoleId').val("");*/
    $('#inputNIK').css('border-color', 'lightgrey');
    $('#inputFirstName').css('border-color', 'lightgrey');
    $('#inputLastName').css('border-color', 'lightgrey');
    $('#inputPhone').css('border-color', 'lightgrey');
    $('#inputBirthDate').css('border-color', 'lightgrey');
    $('#inputSalary').css('border-color', 'lightgrey');
    $('#inputEmail').css('border-color', 'lightgrey');
    $('#inputPassword').css('border-color', 'lightgrey');
    $('#inputDegree').css('border-color', 'lightgrey');
    $('#inputGPA').css('border-color', 'lightgrey');
    $('#inputUnivId').css('border-color', 'lightgrey');
    $('#inputRoleId').css('border-color', 'lightgrey');
}

function registerButton() {
    $('#registerEmployee').modal('show');
}

function exportExcel() {
    $('#dataTable1').DataTable().buttons('excel:name').trigger();
}

function insertData() {
    var obj = new Object();

    obj.NIK = $('#inputNIK').val();
    obj.FirstName = $('#inputFirstName').val();
    obj.LastName = $('#inputLastName').val();
    obj.Phone = $('#inputPhone').val();
    obj.BirthDate = $('#inputBirthDate').val();
    obj.Salary = parseInt($('#inputSalary').val());
    obj.Email = $('#inputEmail').val();
    obj.Password = $('#inputPassword').val();
    obj.Degree = $('#inputDegree').val();
    obj.GPA = $('#inputGPA').val();
    obj.UniversityId = parseInt($('#inputUnivId').val());
    obj.RoleId = parseInt($('#inputRoleId').val());

    console.log(obj);
    $.ajax({
        url: "Employees/Register",
        type: "POST",
        data: { entity: obj},
        dataType: 'json'
    }).done((result) => {
        console.log(result);
        Swal.fire(
            'Added!',
            'Your file has been added.',
            'success'
        );
        $('#dataTable1').DataTable().ajax.reload();
        $('#registerEmployee').modal('hide');
        clearTextBox();
    }).fail((error) => {
        /*if (error.responseJSON.message == 'NIK sudah tersedia') {
            Swal.fire(
                'Failed!',
                'Your NIK has been added before.',
                'error'
            );
        } else if (error.responseJSON.message == 'Email sudah terdaftar') {
            Swal.fire(
                'Failed!',
                'Your Email has been added before.',
                'error'
            );
        } else if (error.responseJSON.message == 'Nomor telepon sudah terdaftar') {
            Swal.fire(
                'Failed!',
                'Your Phone has been added before.',
                'error'
            );
        } else {
            Swal.fire(
                'Failed!',
                'Your file has been fail to added.',
                'error'
            );
        }*/
        console.log(error);
    });
}

function getEmployee(id) {
    console.log(id);
    $.ajax({
        url: "Employees/Get/" + id,
        success: function (result) {
            console.log(result);
            $('#updateEmployee').modal('show');
            $('#nik').val(result.nik);
            $('#firstName').val(result.firstName);
            $('#lastName').val(result.lastName);
            $('#phone').val(result.phone);
            var date = result.birthDate.substr(0, 10);
            $('#birthDate').val(date);
            $('#salary').val(result.salary);
            $('#email').val(result.email);
            if (result.gender == 0) {
                $('#gender').val(0);
            } else {
                $('#gender').val(1);
            }
        }
    })
}

function update(id) {
    console.log(id);
    var obj = new Object();

    obj.nik = id;
    obj.firstName = $('#firstName').val();
    obj.lastName = $('#lastName').val();
    obj.phone = $('#phone').val();
    obj.birthDate = $('#birthDate').val();
    obj.salary = parseInt($('#salary').val());
    obj.email = $('#email').val();
    obj.gender = parseInt($('#gender').val());

    console.log(obj);
    $.ajax({
        url: "Employees/Put/",
        type: "PUT",
        data: { id: id, entity: obj},
        dataType: 'json'
    }).done((result) => {
        console.log(result);
        if (result == 200) {
            Swal.fire(
                'Updated!',
                'Your file has been updated.',
                'success'
            )
            $('#dataTable1').DataTable().ajax.reload();
            $('#updateEmployee').modal('hide');
        }
    }).fail((error) => {
        console.log(error);
        Swal.fire(
            'Failed!',
            'Your file has been fail to updated.',
            'error'
        );
    })
}

function deleteEmployee(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "Employees/Delete/" + id,
                type: "Delete"
            }).then((result) => {
                if (result == 200) {
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted.',
                        'success'
                    )
                    $('#dataTable1').DataTable().ajax.reload();
                }
            })
        }
    })
}