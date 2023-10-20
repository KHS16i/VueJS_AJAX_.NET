const respaldo = new Vue({
    el: '#Respaldo',

    data: {
        cargandoComponente: true,
        cargandoRespaldos: true,

        ddlSist: 0,
        ddlCompSist: 0,
        ddlDev: 0,
        ddlDevCert: 0,

        fechaLib: '',
        fechaMod: '',
        git: false,
        nube: false,
        fisico: false,
        db: false,
        version: '',
        comentario: '',

        listaSist: [],
        listaCompSist: [],
        listaDevs: [],

        listaComponente: [],
        listaRespaldo: [],

        resp: '',
    },


    methods: {
        registrarRespaldo() {
            self = this;

            if (this.$refs.miFormulario.checkValidity()) {
                // Aquí ejecuta la función que deseas realizar cuando el formulario se valida correctamente
                $.post('Ajax.aspx', {
                    opcion: 'AgregarRespaldos',
                    Sist: self.ddlSist,
                    CompSistema: self.ddlCompSist,
                    NombreDev: self.ddlDev,
                    NombreCert: self.ddlDevCert,
                    FechaLib: self.fechaLib,
                    FechaMod: self.fechaMod,
                    Git: self.git,
                    Nube: self.nube,
                    Fisico: self.fisico,
                    DB: self.db,
                    Version: self.version,
                    Comentario: self.comentario,

                }, function (data) {

                    if (data == "Respaldo registrado exitosamente!") {
                        self.obtenerRespaldos();

                        $('#modal_Agregar').modal('hide');
                        self.resetForm();

                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: 'Respaldo agregado con exito!',
                            showConfirmButton: false,
                            timer: 1500
                        })

                    } else {
                        Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'Hubo un error!',
                            showConfirmButton: false,
                            timer: 1500
                        })
                    }
                });
            } else {
                // El formulario no es válido, mostramos las alertas de Bootstrap
                this.$refs.miFormulario.classList.add('was-validated');
            }

        },

        //metodo para  buscar y cargar la tabla
        obtenerComponentes() {

            //se llama al ajax 
            let self = this;
            $.post('Ajax.aspx', {
                opcion: 'ObtenerComponentes'

            }, function (data) {

                self.listaComponente = JSON.parse(data);

                //se crea la tabla de Jquery con los datos asignados
                $(document).ready(function () {
                    $('#tbl_Componente').DataTable({
                        data: self.listaComponente,
                        columns: [
                            { data: 'Empresa' },
                            { data: 'Sistema' },
                            { data: 'Componente' },
                            { data: 'Descripcion' },
                            { data: 'Desarrollador' },
                            { data: 'FechaLiberacion' },
                            { data: 'Version' },
                        ],
                        responsive: true,
                        destroy: true,
                        "language": {
                            "paginate": {
                                "previous": "Anterior",
                                "next": "Siguiente"
                            },
                            "lengthMenu": "Mostrar _MENU_ resultados por pagina",
                            "zeroRecords": "Sin resultados",
                            "info": "Pagina _PAGE_ de _PAGES_",
                            "infoEmpty": "Tabla vacia",
                            "search": "Buscar:",
                            "infoFiltered": "(Filtrando de _MAX_ total de resultados)"
                        },
                    });
                });

                self.cargandoComponente = false;
            });

        },

        obtenerRespaldos: function () {
            let self = this;
            $.post('Ajax.aspx', {
                opcion: 'ObtenerRespaldos'

            }, function (data) {

                self.listaRespaldo = JSON.parse(data);

                //se crea la tabla de Jquery con los datos asignados
                $(document).ready(function () {
                    $('#tbl_reporte').DataTable({
                        data: self.listaRespaldo,
                        columns: [
                            { data: 'Sistema' },
                            { data: 'Componente' },
                            { data: 'Desarrollador' },
                            { data: 'FechaUltimaModificacion' },
                            { data: 'FechaUltimoRespaldo' },
                            { data: 'FechaPruebaRespaldo' },
                            {
                                data: 'RespaldoGIT',
                                render: function (data, type, row) {
                                    return data ?
                                        '<img src="Recursos/check.png" alt="Y">' :
                                        '<img src="Recursos/false.png" alt="N">';
                                }
                            },
                            {
                                data: 'RespaldoNube',
                                render: function (data, type, row) {
                                    return data ?
                                        '<img src="Recursos/check.png" alt="Y">' :
                                        '<img src="Recursos/false.png" alt="N">';
                                }
                            },
                            {
                                data: 'RespaldoFisico',
                                render: function (data, type, row) {
                                    return data ?
                                        '<img src="Recursos/check.png" alt="Y">' :
                                        '<img src="Recursos/false.png" alt="N">';
                                }
                            },
                            {
                                data: 'RespaldoDB',
                                render: function (data, type, row) {
                                    return data ?
                                        '<img src="Recursos/check.png" alt="Y">' :
                                        '<img src="Recursos/false.png" alt="N">';
                                }
                            },
                            { data: 'RespaldoPor' },
                            { data: 'Certificador' },
                            { data: 'Comentarios' },
                        ],
                        responsive: true,
                        destroy: true,
                        "language": {
                            "paginate": {
                                "previous": "Anterior",
                                "next": "Siguiente"
                            },
                            "lengthMenu": "Mostrar _MENU_ resultados por pagina",
                            "zeroRecords": "Sin resultados",
                            "info": "Pagina _PAGE_ de _PAGES_",
                            "infoEmpty": "Tabla vacia",
                            "search": "Buscar:",
                            "infoFiltered": "(Filtrando de _MAX_ total de resultados)"
                        },
                    });
                });

                self.cargandoRespaldos = false;
            });
        },

        handleSelectChange(event) {
            self = this;

            if (this.ddlSist == 0) {
                alert("Debe seleccionar un Sistema");
                return;
            }
            this.cargaCompSist();
        },

        cargaSist: function () {
            let self = this;
            $.post('Ajax.aspx', {
                opcion: 'cargaSist'

            }, function (data) {
                self.listaSist = JSON.parse(data);
            });
        },

        cargaCompSist: function () {
            let self = this;
            $.post('Ajax.aspx', {
                opcion: 'cargaCompSist',
                Sist: self.ddlSist

            }, function (data) {
                self.listaCompSist = JSON.parse(data);
            });
        },

        cargaDevs: function () {
            let self = this;
            $.post('Ajax.aspx', {
                opcion: 'cargaDevs'

            }, function (data) {
                self.listaDevs = JSON.parse(data);
            });
        },

        resetForm: function () {
            self = this;

                this.ddlSist = 0,
                this.ddlCompSist = 0,
                this.ddlDev = 0,
                this.ddlDevCert = 0,

                this.fechaMod = '',
                this.git = false,
                this.nube = false,
                this.fisico = false,
                this.version = '',
                this.comentario = ''
        }
    },
    //Metodo del ciclo de vida que inicia la pagina
    mounted() {
        this.cargaSist();
        this.cargaDevs();
        this.obtenerComponentes();
        this.obtenerRespaldos();
    },
})

/*Para obtener fecha actual en el footer*/
document.addEventListener("DOMContentLoaded", function () {
    const fechaActual = new Date();
    const opcionesFecha = { year: "numeric", month: "long", day: "numeric" };
    const fechaFormateada = fechaActual.toLocaleDateString("es-ES", opcionesFecha);

    const elementoFecha = document.getElementById("fecha-actual");
    elementoFecha.textContent = fechaFormateada;
});

