import { useState } from 'react';
import { Card } from './ui/card';
import { Button } from './ui/button';
import { Input } from './ui/input';
import { Badge } from './ui/badge';
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle, DialogTrigger } from './ui/dialog';
import { Label } from './ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from './ui/select';
import { Tabs, TabsContent, TabsList, TabsTrigger } from './ui/tabs';
import { Avatar, AvatarFallback, AvatarImage } from './ui/avatar';
import { Switch } from './ui/switch';
import { Textarea } from './ui/textarea';
import { Separator } from './ui/separator';
import { 
  UserPlus, 
  Search, 
  Edit, 
  Trash2, 
  Shield, 
  Eye, 
  Phone, 
  Mail, 
  MapPin, 
  Calendar, 
  Activity, 
  Clock, 
  Key,
  Building2,
  Briefcase,
  User,
  FileText
} from 'lucide-react';

interface User {
  id: string;
  nombre: string;
  email: string;
  rol: string;
  empresa: string;
  estado: string;
  ultimoAcceso: string;
  telefono?: string;
  tipoDocumento?: string;
  numeroDocumento?: string;
  direccion?: string;
  fechaCreacion?: string;
  avatar?: string;
  cargo?: string;
  notas?: string;
  permisos?: string[];
}

const users: User[] = [
  { 
    id: '1', 
    nombre: 'Juan Pérez', 
    email: 'juan.perez@empresa.com', 
    rol: 'Administrador', 
    empresa: 'Empresa A', 
    estado: 'activo', 
    ultimoAcceso: '2025-10-22 14:30',
    telefono: '+34 611 222 333',
    tipoDocumento: 'DNI',
    numeroDocumento: '12345678A',
    direccion: 'Calle Mayor 123, Madrid',
    fechaCreacion: '2024-01-15',
    cargo: 'Director de TI',
    permisos: ['Gestión de usuarios', 'Configuración del sistema', 'Reportes avanzados', 'Facturación']
  },
  { 
    id: '2', 
    nombre: 'María García', 
    email: 'maria.garcia@empresa.com', 
    rol: 'Usuario', 
    empresa: 'Empresa B', 
    estado: 'activo', 
    ultimoAcceso: '2025-10-22 13:15',
    telefono: '+34 622 333 444',
    tipoDocumento: 'NIE',
    numeroDocumento: 'X1234567Y',
    direccion: 'Avenida Principal 45, Barcelona',
    fechaCreacion: '2024-03-20',
    cargo: 'Gerente de Ventas',
    permisos: ['Facturación', 'Clientes']
  },
  { 
    id: '3', 
    nombre: 'Carlos López', 
    email: 'carlos.lopez@empresa.com', 
    rol: 'Supervisor', 
    empresa: 'Empresa A', 
    estado: 'activo', 
    ultimoAcceso: '2025-10-21 16:45',
    telefono: '+34 633 444 555',
    tipoDocumento: 'DNI',
    numeroDocumento: '87654321B',
    direccion: 'Plaza España 7, Madrid',
    fechaCreacion: '2024-02-10',
    cargo: 'Supervisor de Operaciones',
    permisos: ['Reportes', 'Gestión de servicios', 'Notificaciones']
  },
  { 
    id: '4', 
    nombre: 'Ana Martínez', 
    email: 'ana.martinez@empresa.com', 
    rol: 'Usuario', 
    empresa: 'Empresa C', 
    estado: 'inactivo', 
    ultimoAcceso: '2025-10-18 10:20',
    telefono: '+34 644 555 666',
    tipoDocumento: 'Pasaporte',
    numeroDocumento: 'AAA123456',
    direccion: 'Calle Secundaria 89, Valencia',
    fechaCreacion: '2024-05-12',
    cargo: 'Asistente Administrativa',
    permisos: ['Facturación']
  },
  { 
    id: '5', 
    nombre: 'Luis Rodríguez', 
    email: 'luis.rodriguez@empresa.com', 
    rol: 'Auditor', 
    empresa: 'Empresa D', 
    estado: 'activo', 
    ultimoAcceso: '2025-10-22 09:00',
    telefono: '+34 655 666 777',
    tipoDocumento: 'DNI',
    numeroDocumento: '11223344C',
    direccion: 'Calle Auditores 15, Sevilla',
    fechaCreacion: '2024-04-01',
    cargo: 'Auditor Senior',
    permisos: ['Reportes avanzados', 'Auditoría']
  },
];

const actividadReciente = [
  { fecha: '2025-10-22 14:30', accion: 'Inicio de sesión', detalle: 'Acceso desde Chrome - Madrid, España' },
  { fecha: '2025-10-22 14:25', accion: 'Actualización de perfil', detalle: 'Cambió número de teléfono' },
  { fecha: '2025-10-22 10:15', accion: 'Generó reporte', detalle: 'Reporte de facturación mensual' },
  { fecha: '2025-10-21 16:45', accion: 'Creó usuario', detalle: 'Nuevo usuario: Pedro Sánchez' },
  { fecha: '2025-10-21 09:30', accion: 'Inicio de sesión', detalle: 'Acceso desde Firefox - Madrid, España' },
];

export function UserManagement() {
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const [showProfile, setShowProfile] = useState(false);
  const [showCreateDialog, setShowCreateDialog] = useState(false);

  const filteredUsers = users.filter(user => 
    user.nombre.toLowerCase().includes(searchTerm.toLowerCase()) ||
    user.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
    user.empresa.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleViewProfile = (user: User) => {
    setSelectedUser(user);
    setShowProfile(true);
  };

  const getInitials = (name: string) => {
    return name.split(' ').map(n => n[0]).join('').toUpperCase();
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-gray-900 mb-1">Gestión de Usuarios</h2>
          <p className="text-gray-500">Administra usuarios, roles y permisos</p>
        </div>
        <Dialog open={showCreateDialog} onOpenChange={setShowCreateDialog}>
          <DialogTrigger asChild>
            <Button className="bg-blue-600 hover:bg-blue-700">
              <UserPlus className="w-4 h-4 mr-2" />
              Nuevo Usuario
            </Button>
          </DialogTrigger>
          <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>Agregar Nuevo Usuario</DialogTitle>
              <DialogDescription>
                Completa la información para crear una nueva cuenta de usuario
              </DialogDescription>
            </DialogHeader>
            <div className="space-y-6 pt-4">
              {/* Información Personal */}
              <div>
                <h3 className="text-gray-900 mb-4">Información Personal</h3>
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <Label>Nombre *</Label>
                    <Input placeholder="Juan" />
                  </div>
                  <div>
                    <Label>Apellidos *</Label>
                    <Input placeholder="Pérez García" />
                  </div>
                  <div>
                    <Label>Tipo de Documento *</Label>
                    <Select>
                      <SelectTrigger>
                        <SelectValue placeholder="Seleccionar tipo" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="dni">DNI</SelectItem>
                        <SelectItem value="nie">NIE</SelectItem>
                        <SelectItem value="pasaporte">Pasaporte</SelectItem>
                        <SelectItem value="cedula">Cédula</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                  <div>
                    <Label>Número de Documento *</Label>
                    <Input placeholder="12345678A" />
                  </div>
                  <div>
                    <Label>Email *</Label>
                    <Input type="email" placeholder="usuario@empresa.com" />
                  </div>
                  <div>
                    <Label>Teléfono</Label>
                    <Input placeholder="+34 611 222 333" />
                  </div>
                  <div className="col-span-2">
                    <Label>Dirección</Label>
                    <Input placeholder="Calle, número, ciudad" />
                  </div>
                </div>
              </div>

              <Separator />

              {/* Información Laboral */}
              <div>
                <h3 className="text-gray-900 mb-4">Información Laboral</h3>
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <Label>Empresa *</Label>
                    <Select>
                      <SelectTrigger>
                        <SelectValue placeholder="Seleccionar empresa" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="empresa-a">Empresa A</SelectItem>
                        <SelectItem value="empresa-b">Empresa B</SelectItem>
                        <SelectItem value="empresa-c">Empresa C</SelectItem>
                        <SelectItem value="empresa-d">Empresa D</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                  <div>
                    <Label>Cargo</Label>
                    <Input placeholder="Ej: Gerente de Ventas" />
                  </div>
                  <div className="col-span-2">
                    <Label>Rol *</Label>
                    <Select>
                      <SelectTrigger>
                        <SelectValue placeholder="Seleccionar rol" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="admin">Administrador</SelectItem>
                        <SelectItem value="supervisor">Supervisor</SelectItem>
                        <SelectItem value="usuario">Usuario</SelectItem>
                        <SelectItem value="auditor">Auditor</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                </div>
              </div>

              <Separator />

              {/* Seguridad */}
              <div>
                <h3 className="text-gray-900 mb-4">Seguridad y Acceso</h3>
                <div className="space-y-4">
                  <div>
                    <Label>Contraseña Temporal *</Label>
                    <Input type="password" placeholder="••••••••" />
                  </div>
                  <div className="flex items-center justify-between">
                    <div>
                      <Label>Requerir cambio de contraseña</Label>
                      <p className="text-sm text-gray-500">El usuario deberá cambiar la contraseña en el primer inicio</p>
                    </div>
                    <Switch defaultChecked />
                  </div>
                  <div className="flex items-center justify-between">
                    <div>
                      <Label>Cuenta activa</Label>
                      <p className="text-sm text-gray-500">El usuario podrá acceder al sistema</p>
                    </div>
                    <Switch defaultChecked />
                  </div>
                </div>
              </div>

              <Separator />

              {/* Notas */}
              <div>
                <Label>Notas adicionales</Label>
                <Textarea placeholder="Información adicional sobre el usuario..." rows={3} />
              </div>

              <div className="flex gap-2 pt-4">
                <Button 
                  variant="outline" 
                  className="flex-1"
                  onClick={() => setShowCreateDialog(false)}
                >
                  Cancelar
                </Button>
                <Button 
                  className="flex-1 bg-blue-600 hover:bg-blue-700"
                  onClick={() => setShowCreateDialog(false)}
                >
                  Crear Usuario
                </Button>
              </div>
            </div>
          </DialogContent>
        </Dialog>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
        <Card className="p-6">
          <p className="text-sm text-gray-600 mb-1">Usuarios Totales</p>
          <h3 className="text-gray-900">152</h3>
        </Card>
        <Card className="p-6">
          <p className="text-sm text-gray-600 mb-1">Usuarios Activos</p>
          <h3 className="text-gray-900">148</h3>
        </Card>
        <Card className="p-6">
          <p className="text-sm text-gray-600 mb-1">Administradores</p>
          <h3 className="text-gray-900">12</h3>
        </Card>
        <Card className="p-6">
          <p className="text-sm text-gray-600 mb-1">Nuevos (7 días)</p>
          <h3 className="text-gray-900">8</h3>
        </Card>
      </div>

      {/* Search and Filter */}
      <Card className="p-6">
        <div className="flex items-center gap-4">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400" />
            <Input 
              placeholder="Buscar usuarios por nombre, email o empresa..." 
              className="pl-10"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <Select>
            <SelectTrigger className="w-40">
              <SelectValue placeholder="Filtrar por rol" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">Todos</SelectItem>
              <SelectItem value="admin">Administrador</SelectItem>
              <SelectItem value="supervisor">Supervisor</SelectItem>
              <SelectItem value="usuario">Usuario</SelectItem>
              <SelectItem value="auditor">Auditor</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </Card>

      {/* Users Table */}
      <Card className="p-6">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead>
              <tr className="border-b">
                <th className="text-left py-3 px-4 text-gray-600">Usuario</th>
                <th className="text-left py-3 px-4 text-gray-600">Empresa</th>
                <th className="text-left py-3 px-4 text-gray-600">Rol</th>
                <th className="text-left py-3 px-4 text-gray-600">Estado</th>
                <th className="text-left py-3 px-4 text-gray-600">Último Acceso</th>
                <th className="text-left py-3 px-4 text-gray-600">Acciones</th>
              </tr>
            </thead>
            <tbody>
              {filteredUsers.map((user) => (
                <tr key={user.id} className="border-b hover:bg-gray-50">
                  <td className="py-3 px-4">
                    <div className="flex items-center gap-3">
                      <Avatar>
                        <AvatarImage src={user.avatar} />
                        <AvatarFallback>{getInitials(user.nombre)}</AvatarFallback>
                      </Avatar>
                      <div>
                        <p className="text-gray-900">{user.nombre}</p>
                        <p className="text-sm text-gray-500">{user.email}</p>
                      </div>
                    </div>
                  </td>
                  <td className="py-3 px-4 text-gray-900">{user.empresa}</td>
                  <td className="py-3 px-4">
                    <Badge variant="outline" className="gap-1">
                      <Shield className="w-3 h-3" />
                      {user.rol}
                    </Badge>
                  </td>
                  <td className="py-3 px-4">
                    {user.estado === 'activo' ? (
                      <Badge className="bg-green-100 text-green-700 hover:bg-green-100">Activo</Badge>
                    ) : (
                      <Badge className="bg-gray-100 text-gray-700 hover:bg-gray-100">Inactivo</Badge>
                    )}
                  </td>
                  <td className="py-3 px-4 text-gray-600">{user.ultimoAcceso}</td>
                  <td className="py-3 px-4">
                    <div className="flex items-center gap-2">
                      <Button 
                        variant="ghost" 
                        size="icon"
                        onClick={() => handleViewProfile(user)}
                      >
                        <Eye className="w-4 h-4" />
                      </Button>
                      <Button variant="ghost" size="icon">
                        <Edit className="w-4 h-4" />
                      </Button>
                      <Button variant="ghost" size="icon">
                        <Trash2 className="w-4 h-4 text-red-600" />
                      </Button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </Card>

      {/* User Profile Dialog */}
      <Dialog open={showProfile} onOpenChange={setShowProfile}>
        <DialogContent className="max-w-4xl max-h-[90vh] overflow-y-auto">
          {selectedUser && (
            <>
              <DialogHeader>
                <div className="flex items-center gap-4">
                  <Avatar className="w-16 h-16">
                    <AvatarImage src={selectedUser.avatar} />
                    <AvatarFallback>{getInitials(selectedUser.nombre)}</AvatarFallback>
                  </Avatar>
                  <div>
                    <DialogTitle>{selectedUser.nombre}</DialogTitle>
                    <DialogDescription>
                      {selectedUser.cargo || selectedUser.rol} - {selectedUser.empresa}
                    </DialogDescription>
                  </div>
                </div>
              </DialogHeader>

              <Tabs defaultValue="general" className="mt-6">
                <TabsList className="grid w-full grid-cols-3">
                  <TabsTrigger value="general">Información General</TabsTrigger>
                  <TabsTrigger value="permisos">Permisos</TabsTrigger>
                  <TabsTrigger value="actividad">Actividad</TabsTrigger>
                </TabsList>

                {/* General Tab */}
                <TabsContent value="general" className="space-y-6 mt-6">
                  <div className="grid grid-cols-2 gap-6">
                    {/* Información Personal */}
                    <div className="space-y-4">
                      <h3 className="text-gray-900 flex items-center gap-2">
                        <User className="w-5 h-5" />
                        Información Personal
                      </h3>
                      <div className="space-y-3 pl-7">
                        {selectedUser.tipoDocumento && selectedUser.numeroDocumento && (
                          <div className="flex items-center gap-2 text-sm">
                            <FileText className="w-4 h-4 text-gray-400" />
                            <span className="text-gray-600">Documento:</span>
                            <span className="text-gray-900">{selectedUser.tipoDocumento} - {selectedUser.numeroDocumento}</span>
                          </div>
                        )}
                        <div className="flex items-center gap-2 text-sm">
                          <Mail className="w-4 h-4 text-gray-400" />
                          <span className="text-gray-600">Email:</span>
                          <span className="text-gray-900">{selectedUser.email}</span>
                        </div>
                        {selectedUser.telefono && (
                          <div className="flex items-center gap-2 text-sm">
                            <Phone className="w-4 h-4 text-gray-400" />
                            <span className="text-gray-600">Teléfono:</span>
                            <span className="text-gray-900">{selectedUser.telefono}</span>
                          </div>
                        )}
                        {selectedUser.direccion && (
                          <div className="flex items-start gap-2 text-sm">
                            <MapPin className="w-4 h-4 text-gray-400 mt-0.5" />
                            <span className="text-gray-600">Dirección:</span>
                            <span className="text-gray-900">{selectedUser.direccion}</span>
                          </div>
                        )}
                      </div>
                    </div>

                    {/* Información Laboral */}
                    <div className="space-y-4">
                      <h3 className="text-gray-900 flex items-center gap-2">
                        <Briefcase className="w-5 h-5" />
                        Información Laboral
                      </h3>
                      <div className="space-y-3 pl-7">
                        <div className="flex items-center gap-2 text-sm">
                          <Building2 className="w-4 h-4 text-gray-400" />
                          <span className="text-gray-600">Empresa:</span>
                          <span className="text-gray-900">{selectedUser.empresa}</span>
                        </div>
                        <div className="flex items-center gap-2 text-sm">
                          <Shield className="w-4 h-4 text-gray-400" />
                          <span className="text-gray-600">Rol:</span>
                          <Badge variant="outline">{selectedUser.rol}</Badge>
                        </div>
                        {selectedUser.cargo && (
                          <div className="flex items-center gap-2 text-sm">
                            <User className="w-4 h-4 text-gray-400" />
                            <span className="text-gray-600">Cargo:</span>
                            <span className="text-gray-900">{selectedUser.cargo}</span>
                          </div>
                        )}
                      </div>
                    </div>
                  </div>

                  <Separator />

                  {/* Estado de la Cuenta */}
                  <div>
                    <h3 className="text-gray-900 mb-4 flex items-center gap-2">
                      <Activity className="w-5 h-5" />
                      Estado de la Cuenta
                    </h3>
                    <div className="grid grid-cols-2 gap-4 pl-7">
                      <Card className="p-4">
                        <p className="text-sm text-gray-600 mb-1">Estado</p>
                        {selectedUser.estado === 'activo' ? (
                          <Badge className="bg-green-100 text-green-700 hover:bg-green-100">Activo</Badge>
                        ) : (
                          <Badge className="bg-gray-100 text-gray-700 hover:bg-gray-100">Inactivo</Badge>
                        )}
                      </Card>
                      <Card className="p-4">
                        <p className="text-sm text-gray-600 mb-1">Último acceso</p>
                        <p className="text-sm text-gray-900">{selectedUser.ultimoAcceso}</p>
                      </Card>
                      <Card className="p-4">
                        <p className="text-sm text-gray-600 mb-1">Fecha de creación</p>
                        <p className="text-sm text-gray-900">{selectedUser.fechaCreacion || 'N/A'}</p>
                      </Card>
                      <Card className="p-4">
                        <p className="text-sm text-gray-600 mb-1">Sesiones activas</p>
                        <p className="text-sm text-gray-900">2 dispositivos</p>
                      </Card>
                    </div>
                  </div>
                </TabsContent>

                {/* Permisos Tab */}
                <TabsContent value="permisos" className="space-y-6 mt-6">
                  <div className="flex items-center justify-between">
                    <div>
                      <h3 className="text-gray-900 flex items-center gap-2">
                        <Key className="w-5 h-5" />
                        Permisos y Accesos por Módulo
                      </h3>
                      <p className="text-sm text-gray-500 mt-1">Habilita o deshabilita permisos específicos para cada módulo</p>
                    </div>
                  </div>
                  
                  {/* Dashboard */}
                  <Card className="overflow-hidden">
                    <div className="bg-gradient-to-r from-blue-50 to-blue-100 border-b p-4">
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 bg-blue-600 rounded-lg flex items-center justify-center">
                            <Activity className="w-5 h-5 text-white" />
                          </div>
                          <div>
                            <h4 className="text-gray-900">Dashboard</h4>
                            <p className="text-sm text-gray-600">Panel principal y estadísticas</p>
                          </div>
                        </div>
                        <Switch defaultChecked />
                      </div>
                    </div>
                    <div className="p-4 space-y-3">
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver estadísticas generales</p>
                          <p className="text-xs text-gray-500">Acceso a métricas y KPIs principales</p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver gráficos avanzados</p>
                          <p className="text-xs text-gray-500">Visualizaciones detalladas de datos</p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Exportar datos</p>
                          <p className="text-xs text-gray-500">Descargar información en diferentes formatos</p>
                        </div>
                        <Switch />
                      </div>
                    </div>
                  </Card>

                  {/* Gestión de Usuarios */}
                  <Card className="overflow-hidden">
                    <div className="bg-gradient-to-r from-purple-50 to-purple-100 border-b p-4">
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 bg-purple-600 rounded-lg flex items-center justify-center">
                            <User className="w-5 h-5 text-white" />
                          </div>
                          <div>
                            <h4 className="text-gray-900">Gestión de Usuarios</h4>
                            <p className="text-sm text-gray-600">Administración de cuentas y permisos</p>
                          </div>
                        </div>
                        <Switch defaultChecked={selectedUser.permisos?.some(p => p.toLowerCase().includes('usuario'))} />
                      </div>
                    </div>
                    <div className="p-4 space-y-3">
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver usuarios</p>
                          <p className="text-xs text-gray-500">Consultar lista de usuarios</p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Crear usuarios</p>
                          <p className="text-xs text-gray-500">Agregar nuevos usuarios al sistema</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Editar usuarios</p>
                          <p className="text-xs text-gray-500">Modificar información de usuarios</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Eliminar usuarios</p>
                          <p className="text-xs text-gray-500">Remover usuarios del sistema</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Gestionar permisos</p>
                          <p className="text-xs text-gray-500">Asignar y modificar permisos</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                    </div>
                  </Card>

                  {/* Servicios */}
                  <Card className="overflow-hidden">
                    <div className="bg-gradient-to-r from-green-50 to-green-100 border-b p-4">
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 bg-green-600 rounded-lg flex items-center justify-center">
                            <Shield className="w-5 h-5 text-white" />
                          </div>
                          <div>
                            <h4 className="text-gray-900">Servicios</h4>
                            <p className="text-sm text-gray-600">Gestión de servicios registrados</p>
                          </div>
                        </div>
                        <Switch defaultChecked={selectedUser.permisos?.some(p => p.toLowerCase().includes('servicio'))} />
                      </div>
                    </div>
                    <div className="p-4 space-y-3">
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver servicios</p>
                          <p className="text-xs text-gray-500">Consultar servicios registrados</p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Registrar servicios</p>
                          <p className="text-xs text-gray-500">Agregar nuevos servicios</p>
                        </div>
                        <Switch />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Editar servicios</p>
                          <p className="text-xs text-gray-500">Modificar información de servicios</p>
                        </div>
                        <Switch />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Descargar/Imprimir</p>
                          <p className="text-xs text-gray-500">Exportar información de servicios</p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                    </div>
                  </Card>

                  {/* Reportes y Auditoría */}
                  <Card className="overflow-hidden">
                    <div className="bg-gradient-to-r from-orange-50 to-orange-100 border-b p-4">
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 bg-orange-600 rounded-lg flex items-center justify-center">
                            <Activity className="w-5 h-5 text-white" />
                          </div>
                          <div>
                            <h4 className="text-gray-900">Reportes y Auditoría</h4>
                            <p className="text-sm text-gray-600">Informes y seguimiento de actividad</p>
                          </div>
                        </div>
                        <Switch defaultChecked={selectedUser.permisos?.some(p => p.toLowerCase().includes('reporte'))} />
                      </div>
                    </div>
                    <div className="p-4 space-y-3">
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver reportes básicos</p>
                          <p className="text-xs text-gray-500">Consultar reportes estándar</p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver reportes avanzados</p>
                          <p className="text-xs text-gray-500">Acceso a reportes detallados</p>
                        </div>
                        <Switch defaultChecked={selectedUser.permisos?.some(p => p.toLowerCase().includes('avanzado'))} />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Generar reportes personalizados</p>
                          <p className="text-xs text-gray-500">Crear reportes con filtros específicos</p>
                        </div>
                        <Switch />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver auditoría del sistema</p>
                          <p className="text-xs text-gray-500">Seguimiento de cambios y actividad</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                    </div>
                  </Card>

                  {/* Notificaciones */}
                  <Card className="overflow-hidden">
                    <div className="bg-gradient-to-r from-yellow-50 to-yellow-100 border-b p-4">
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 bg-yellow-600 rounded-lg flex items-center justify-center">
                            <Mail className="w-5 h-5 text-white" />
                          </div>
                          <div>
                            <h4 className="text-gray-900">Notificaciones</h4>
                            <p className="text-sm text-gray-600">Alertas y comunicaciones</p>
                          </div>
                        </div>
                        <Switch defaultChecked={selectedUser.permisos?.some(p => p.toLowerCase().includes('notificacion'))} />
                      </div>
                    </div>
                    <div className="p-4 space-y-3">
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Recibir notificaciones</p>
                          <p className="text-xs text-gray-500">Alertas del sistema</p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Enviar notificaciones</p>
                          <p className="text-xs text-gray-500">Crear y enviar alertas</p>
                        </div>
                        <Switch />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Gestionar plantillas</p>
                          <p className="text-xs text-gray-500">Crear y editar plantillas de notificaciones</p>
                        </div>
                        <Switch />
                      </div>
                    </div>
                  </Card>

                  {/* Multiempresa */}
                  <Card className="overflow-hidden">
                    <div className="bg-gradient-to-r from-indigo-50 to-indigo-100 border-b p-4">
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 bg-indigo-600 rounded-lg flex items-center justify-center">
                            <Building2 className="w-5 h-5 text-white" />
                          </div>
                          <div>
                            <h4 className="text-gray-900">Multiempresa</h4>
                            <p className="text-sm text-gray-600">Gestión de múltiples empresas</p>
                          </div>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                    </div>
                    <div className="p-4 space-y-3">
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver todas las empresas</p>
                          <p className="text-xs text-gray-500">Acceso a información de todas las empresas</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Crear empresas</p>
                          <p className="text-xs text-gray-500">Agregar nuevas empresas al sistema</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Cambiar entre empresas</p>
                          <p className="text-xs text-gray-500">Alternar entre empresas asignadas</p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                    </div>
                  </Card>

                  {/* Configuración del Sistema */}
                  <Card className="overflow-hidden">
                    <div className="bg-gradient-to-r from-red-50 to-red-100 border-b p-4">
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 bg-red-600 rounded-lg flex items-center justify-center">
                            <Shield className="w-5 h-5 text-white" />
                          </div>
                          <div>
                            <h4 className="text-gray-900">Configuración del Sistema</h4>
                            <p className="text-sm text-gray-600">Ajustes y parámetros generales</p>
                          </div>
                        </div>
                        <Switch defaultChecked={selectedUser.permisos?.some(p => p.toLowerCase().includes('configuración') || p.toLowerCase().includes('sistema'))} />
                      </div>
                    </div>
                    <div className="p-4 space-y-3">
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Ver configuración</p>
                          <p className="text-xs text-gray-500">Consultar parámetros del sistema</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Modificar configuración</p>
                          <p className="text-xs text-gray-500">Cambiar ajustes del sistema</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                      <Separator />
                      <div className="flex items-center justify-between">
                        <div className="flex-1">
                          <p className="text-sm text-gray-900">Gestionar integraciones</p>
                          <p className="text-xs text-gray-500">Configurar conexiones externas</p>
                        </div>
                        <Switch defaultChecked={selectedUser.rol === 'Administrador'} />
                      </div>
                    </div>
                  </Card>

                  <div className="flex justify-end gap-2 pt-4">
                    <Button variant="outline">
                      Cancelar
                    </Button>
                    <Button className="bg-blue-600 hover:bg-blue-700">
                      <Shield className="w-4 h-4 mr-2" />
                      Guardar Permisos
                    </Button>
                  </div>
                </TabsContent>

                {/* Actividad Tab */}
                <TabsContent value="actividad" className="space-y-4 mt-6">
                  <h3 className="text-gray-900 flex items-center gap-2">
                    <Clock className="w-5 h-5" />
                    Actividad Reciente
                  </h3>
                  
                  <Card className="p-6">
                    <div className="space-y-4">
                      {actividadReciente.map((actividad, index) => (
                        <div key={index} className="flex gap-4 pb-4 border-b last:border-0 last:pb-0">
                          <div className="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center flex-shrink-0">
                            <Activity className="w-5 h-5 text-blue-600" />
                          </div>
                          <div className="flex-1">
                            <div className="flex items-start justify-between">
                              <div>
                                <p className="text-gray-900">{actividad.accion}</p>
                                <p className="text-sm text-gray-500 mt-1">{actividad.detalle}</p>
                              </div>
                              <span className="text-sm text-gray-500">{actividad.fecha}</span>
                            </div>
                          </div>
                        </div>
                      ))}
                    </div>
                  </Card>

                  <div className="grid grid-cols-3 gap-4">
                    <Card className="p-4">
                      <p className="text-sm text-gray-600 mb-1">Total de sesiones</p>
                      <h3 className="text-gray-900">248</h3>
                    </Card>
                    <Card className="p-4">
                      <p className="text-sm text-gray-600 mb-1">Acciones realizadas</p>
                      <h3 className="text-gray-900">1,542</h3>
                    </Card>
                    <Card className="p-4">
                      <p className="text-sm text-gray-600 mb-1">Tiempo promedio</p>
                      <h3 className="text-gray-900">2h 15m</h3>
                    </Card>
                  </div>
                </TabsContent>
              </Tabs>

              <div className="flex justify-end gap-2 mt-6 pt-4 border-t">
                <Button variant="outline" onClick={() => setShowProfile(false)}>
                  Cerrar
                </Button>
                <Button className="bg-blue-600 hover:bg-blue-700">
                  <Edit className="w-4 h-4 mr-2" />
                  Editar Usuario
                </Button>
              </div>
            </>
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}